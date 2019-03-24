using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Interfaces;
using UnityEngine.UI;

public enum BlockType {T = 0, L = 1, Z = 2}

public class GameStateMachine : MonoBehaviour
{
    [Header("Developer Mode")]
    public bool devMode;

    [Header("Needed References")]
    public GameObject canvas_Ref;
    public GameObject bordersParent_Ref;
    public GameObject playerOneParent_Ref;
    public GameObject playerTwoParent_Ref;
    public GameObject playerOneSpawner_Ref;
    public GameObject playerTwoSpawner_Ref;
    public GameObject introTauntScreen_Ref;
    public GameObject scoreScreen_Ref;
    public BubbleManager bubbleManager_Ref;
    public Text inGameText_Ref;
    public PlayerInfoUI playerInfoOne_Ref;
    public PlayerInfoUI playerInfoTwo_Ref;
    public InGameUIManager inGameUIManager_Ref;
    public GameObject[] blockPrefab_Refs;
    public GameObject staticObjects_Ref;

    public PlayerInput playerOneInput_Ref;
    public PlayerInput playerTwoInput_Ref;

    public GameObject disclaimerText;
    public GameObject enableDevModeText;

    [Header("Settings")]
    public bool debugMode;
    public float timeToWaitBeforeScoreScreen;
    public int gamesPlayedBeforeRefill;

   
    private IStateBase gameState;


    private static GameStateMachine gameStateMachine_Ref;
    private Gameboard gameboard_Ref;

    private Text debugTxtReference;
    private string currentState;

    private PlayerCharacter playerOneCharacter;
    private PlayerCharacter playerTwoCharacter;
    private PlayerCharacter[] playerCharacterArray;

    private Object[] portrait_Refs;
    private Object[] gamerTag_Refs;
    private Object[] introPhrases;
    private Object[] victoryPhrases;

    private bool[] playerOneBlocksCollected;
    private bool[] playerTwoBlocksCollected;
    private bool introIsDone;
    private bool gameOver;
    private bool playerOneWon;
    private int playerOneBlocksLanded;
    private int playerTwoBlocksLanded;

    [HideInInspector] public bool isFirstGame { get; set; }
    [HideInInspector] public Vector2 playerOneSpawnPosition;
    [HideInInspector] public Vector2 playerTwoSpawnPosition;

    private int gamesPlayed;

    public static GameStateMachine GetInstance()
    {
        return gameStateMachine_Ref;
    }

    void Awake()
    {
        if (gameStateMachine_Ref == null)
        {
            gameStateMachine_Ref = this;
        }
        else
        {
            Destroy(this);
        }

       gameboard_Ref = Gameboard.GetInstance();

        isFirstGame = true;
        if (devMode)
        {
            portrait_Refs = Resources.LoadAll("Portraits", typeof(Sprite));
            if (disclaimerText != null)
                disclaimerText.SetActive(true);
        }
        else
        {
            portrait_Refs = Resources.LoadAll("WitchPortraits", typeof(Sprite));
            if (enableDevModeText != null)
                enableDevModeText.SetActive(true);

        }
        gamerTag_Refs = Resources.LoadAll("GamerTags", typeof(TextAsset));
        introPhrases = Resources.LoadAll("IntroPhrases", typeof(TextAsset));
        victoryPhrases = Resources.LoadAll("VictoryPhrases", typeof(TextAsset));

        playerOneCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases, gamerTag_Refs);
        playerTwoCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases, gamerTag_Refs);
        playerCharacterArray = new PlayerCharacter[] { playerOneCharacter, playerTwoCharacter};


    }
    void Start()
    {
        gameStateMachine_Ref = this;

        if (canvas_Ref == null)
        {
            if (transform.GetChild(0).gameObject != null)
            {
                canvas_Ref = transform.GetChild(0).gameObject;
            }
            else
            {
                Debug.Log("GameStateMachine is missing a reference to the Canvas");
            }
        }


        if (gameState == null)
        {
            gameState = new BeginState(this);
        }

        if (bordersParent_Ref == null)
        {
            if (GameObject.Find("BordersParent") != null)
            {
                bordersParent_Ref = GameObject.Find("BordersParent");
            }
            else
            {
                Debug.Log("GameStateMachine is missing a reference to BordersParent");
            }
        }

        if (playerInfoOne_Ref == null)
            Debug.Log("GameStateMachine is missing a reference to playerOneInfo");


        if (playerInfoTwo_Ref == null)
            Debug.Log("GameStateMachine is missing a reference to playerTwoInfo");

        playerOneSpawner_Ref.GetComponent<Spawner>().PlayerParent = playerOneParent_Ref;
        playerTwoSpawner_Ref.GetComponent<Spawner>().PlayerParent = playerTwoParent_Ref;

        gameState.ShowIt();

        playerOneBlocksCollected = new bool[playerOneSpawner_Ref.GetComponent<Spawner>().groups.Length];
        playerTwoBlocksCollected = new bool[playerTwoSpawner_Ref.GetComponent<Spawner>().groups.Length];

       if(gamesPlayedBeforeRefill == 0) { gamesPlayedBeforeRefill = 28; }
        inGameUIManager_Ref.UpdatePlayerStatus(WhoIsInTheLead());
    }

    void Update()
    {
        gameState.StateUpdate();
    }
    public void ChangeState(IStateBase newState)
    {
        gameState = newState;
        gameState.ShowIt();
    }
    public void TurnOnCanvasSection(int child)
    {
        //the argument child will be the canvas child index and that child will be set active
        // all the other childs will be set inactive.

        int canvasChildLength = canvas_Ref.transform.childCount;

        for (int i = 0; i < canvasChildLength; i++)
        {
            canvas_Ref.transform.GetChild(i).gameObject.SetActive(false);
        }

        canvas_Ref.transform.GetChild(child).gameObject.SetActive(true);

    }
    public void ToggleSpawners(bool b)
    {


        if (playerOneSpawner_Ref != null)
        {
            playerOneSpawner_Ref.GetComponent<Spawner>().playerSpawnpoint = FindSpawnPosition(true);
            playerOneSpawner_Ref.SetActive(b);
        }
        else
        {
            Debug.Log("GameStateMachine is missing a reference to player ONE's spawner!");
        }

        if (playerTwoSpawner_Ref != null)
        {
            playerTwoSpawner_Ref.GetComponent<Spawner>().playerSpawnpoint = FindSpawnPosition(false);
            playerTwoSpawner_Ref.SetActive(b);
        }
        else
        {
            Debug.Log("GameStateMachine is missing a reference to player TWO's spawner!");
        }
    }
    private Vector2 FindSpawnPosition(bool playerOneSide)
    {
        if (playerOneSide)
        {
            float topLeftX = bubbleManager_Ref.p1_LeftTop.x;
            float topRightX = bubbleManager_Ref.p1_RightTop.x;

            float middleX = Mathf.Abs((topLeftX - topRightX) / 2) - topRightX;

            Vector2 vectorToReturn = new Vector2(middleX * -1, bubbleManager_Ref.p2_LeftTop.y -1.5f);

            playerOneSpawnPosition = vectorToReturn;

            return vectorToReturn;
        }
        else
        {
            float topLeftX = bubbleManager_Ref.p2_LeftTop.x;
            float topRightX = bubbleManager_Ref.p2_RightTop.x;

            float middleX = Mathf.Abs((topLeftX - topRightX) / 2) + topLeftX;

            Vector2 vectorToReturn = new Vector2(middleX, bubbleManager_Ref.p2_LeftTop.y - 1.5f);

            playerTwoSpawnPosition = vectorToReturn;

            return vectorToReturn;
        }
    }
    public void CrashedBlock(bool isPlayerOne)
    {
        if (isPlayerOne)
        {
            bubbleManager_Ref.AddPeneltyPoint(true);
        }
        else
        {
            bubbleManager_Ref.AddPeneltyPoint(false);
        }
    }
    public bool Collected(bool isPlayerOne, int blockCollected)
    {

        if (isPlayerOne)
        {
            playerOneBlocksCollected[blockCollected] = true;
            inGameUIManager_Ref.ActivateImage(true, blockCollected);
            playerOneBlocksLanded++;
        }
        else
        {
            playerTwoBlocksCollected[blockCollected] = true;
            inGameUIManager_Ref.ActivateImage(false, blockCollected);
            playerTwoBlocksLanded++;
        }

        inGameUIManager_Ref.UpdatePlayerStatus(WhoIsInTheLead());

        return CheckIfSomeoneWon();
    }
    private bool CheckIfSomeoneWon()
    {
        bool playerOneWon = true;
        bool playerTwoWon = true;

        foreach (bool b in playerOneBlocksCollected)
        {
            if (!b)
            {

                playerOneWon = false; break;
            }
        }

        foreach (bool b in playerTwoBlocksCollected)
        {
            if (!b)
            {
                playerTwoWon = false; break;
            }
        }

        if (playerOneWon)
        {
            WeHaveAWinner(true);
            return true;
        }

        else if (playerTwoWon)
        {
            WeHaveAWinner(false);
            return true;
        }
        else
        {
            return false;
        }
    }
    private void WeHaveAWinner(bool isPlayerOne)
    {
        if (!gameOver)
        {
            gameOver = true;
            bubbleManager_Ref.isOn = false;

            if (isPlayerOne)
            {
                playerOneWon = true;
                inGameText_Ref.text = "Player One Wins!";
                inGameText_Ref.transform.parent.gameObject.SetActive(true);


            }
            else
            {
                playerOneWon = false;
                inGameText_Ref.text = "Player Two Wins!";
                inGameText_Ref.transform.parent.gameObject.SetActive(true);


            }
        }

        Invoke("ChangeToScoreScreen", timeToWaitBeforeScoreScreen);
    }
    private void ChangeToScoreScreen()
    {
        bubbleManager_Ref.TurnOff();
        ChangeState(new ScoreScreenState(this));
    }
    public void TurnOffIntroTauntScreen()
    {
        introTauntScreen_Ref.SetActive(false);
    }
    public void Reset()
    { 

       

        gameStateMachine_Ref = this;

        if (canvas_Ref == null)
        {
            if (transform.GetChild(0).gameObject != null)
            {
                canvas_Ref = transform.GetChild(0).gameObject;
            }
            else
            {
                Debug.Log("GameStateMachine is missing a reference to the Canvas");
            }
        }

        if (bordersParent_Ref == null)
        {
            if (GameObject.Find("BordersParent") != null)
            {
                bordersParent_Ref = GameObject.Find("BordersParent");
            }
            else
            {
                Debug.Log("GameStateMachine is missing a reference to BordersParent");
            }
        }

        if (playerInfoOne_Ref == null)
            Debug.Log("GameStateMachine is missing a reference to playerOneInfo");


        if (playerInfoTwo_Ref == null)
            Debug.Log("GameStateMachine is missing a reference to playerTwoInfo");
        
        //when games played are more than the gamesPlayedBeforeRefill this
        //if statemant will refill the arrays that hold portraits and phrases.
        if (gamesPlayed > gamesPlayedBeforeRefill || !devMode)
        {
            if (devMode)
            {
                portrait_Refs = Resources.LoadAll("Portraits", typeof(Sprite));
            }
            else
            {
                portrait_Refs = Resources.LoadAll("WitchPortraits", typeof(Sprite));
            }
            gamerTag_Refs = Resources.LoadAll("GamerTags", typeof(TextAsset));
            introPhrases = Resources.LoadAll("IntroPhrases", typeof(TextAsset));
            victoryPhrases = Resources.LoadAll("VictoryPhrases", typeof(TextAsset));

            gamesPlayed = -1;
        }

        playerOneCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases, gamerTag_Refs);
        playerTwoCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases, gamerTag_Refs);
        playerCharacterArray = new PlayerCharacter[] { playerOneCharacter, playerTwoCharacter };

        playerOneBlocksCollected = new bool[playerOneSpawner_Ref.GetComponent<Spawner>().groups.Length];
        playerOneBlocksLanded = 0;
        playerTwoBlocksCollected = new bool[playerTwoSpawner_Ref.GetComponent<Spawner>().groups.Length];
        playerTwoBlocksLanded = 0;
        gameOver = false;
        playerOneWon = false;
        introIsDone = false;
        inGameUIManager_Ref.ResetCollectedBlocks();
        inGameUIManager_Ref.UpdatePlayerDisplays();
        inGameText_Ref.text = "Get Ready!";
        RefillSpawners();

        playerOneSpawner_Ref.GetComponent<Spawner>().RemoveRemainingBlocks();
        playerTwoSpawner_Ref.GetComponent<Spawner>().RemoveRemainingBlocks();

        inGameUIManager_Ref.UpdatePlayerStatus(WhoIsInTheLead());

        gamesPlayed++;

    }

    public void GetAllNeededReferences()
    {
       if(inGameUIManager_Ref == null)
            inGameUIManager_Ref = InGameUIManager.GetInstance();
        if (bubbleManager_Ref == null)
            bubbleManager_Ref = BubbleManager.GetInstance();

    }
    public void FastWin()
    {
        WeHaveAWinner(true);
    }
    private void RefillSpawners()
    {
        
        
            for (int i = 0; i < playerOneSpawner_Ref.GetComponent<Spawner>().groups.Length; i++)
            {
                if (blockPrefab_Refs[i] != null)
                {
                    playerOneSpawner_Ref.GetComponent<Spawner>().groups[i] = blockPrefab_Refs[i];
                    playerTwoSpawner_Ref.GetComponent<Spawner>().groups[i] = blockPrefab_Refs[i];
                }
                else
                {
                    Debug.Log("blocksPrefab_Refs with index: " + i + ", does not have a value.");
                }

            }
        
       

        isFirstGame = false;

    }
    public void WaitAndChangeToBeginState()
    {
        Invoke("Restart", 3.0f);
    }
    private void Restart()
    {
        ChangeState(new BeginState(this));
    }

    public int WhoIsInTheLead()
    {
        if(playerOneBlocksLanded > PlayerTwoBlocksLanded)
        {
            return 1;
        }
        else if(playerOneBlocksLanded < PlayerTwoBlocksLanded)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }
    public void LoadDevMode()
    {
        devMode = true;

        disclaimerText.SetActive(true);
        enableDevModeText.SetActive(false);

        portrait_Refs = Resources.LoadAll("Portraits", typeof(Sprite));

        playerOneCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases, gamerTag_Refs);
        playerTwoCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases, gamerTag_Refs);
        playerCharacterArray = new PlayerCharacter[] { playerOneCharacter, playerTwoCharacter };

        inGameUIManager_Ref.UpdatePlayerDisplays();
    }
    public void LoadShipMode()
    {
        devMode = false;

        disclaimerText.SetActive(false);
        enableDevModeText.SetActive(true);

        portrait_Refs = portrait_Refs = Resources.LoadAll("WitchPortraits", typeof(Sprite));

        playerOneCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases, gamerTag_Refs);
        playerTwoCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases, gamerTag_Refs);
        playerCharacterArray = new PlayerCharacter[] { playerOneCharacter, playerTwoCharacter };

        inGameUIManager_Ref.UpdatePlayerDisplays();
    }

    // properties
    public GameObject Canvas_Ref {get {return canvas_Ref;}}
    public BubbleManager BubbleManager_Ref { set { bubbleManager_Ref = value; } get { return bubbleManager_Ref; } }
    public PlayerCharacter[] PlayerCharacterArray { get { return playerCharacterArray; } }
    public bool IntroIsDone { get { return introIsDone; } set { introIsDone = value; } }
    public bool PlayerOneWon { get { return playerOneWon; } }
    public bool GameOver { get { return gameOver; } }
    public int PlayerOneBlocksLanded { get { return playerOneBlocksLanded; } }
    public int PlayerTwoBlocksLanded { get { return playerTwoBlocksLanded; } }

}
