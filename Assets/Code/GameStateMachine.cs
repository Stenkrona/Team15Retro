using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Interfaces;
using UnityEngine.UI;

public enum BlockType {I = 0, J = 1, L = 2, O = 3, S = 4, T = 5, Z = 6}

public class GameStateMachine : MonoBehaviour
{

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

    public PlayerInput playerOneInput_Ref;
    public PlayerInput playerTwoInput_Ref;

    [Header("Settings")]
    public bool debugMode;
    public float timeToWaitBeforeScoreScreen;
    public int gamesPlayedBeforeRefill;

    private IStateBase gameState;


    private static GameStateMachine gameStateMachine_Ref;

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
    [HideInInspector] public bool isFirstGame { get; set; }

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

        isFirstGame = true;

        portrait_Refs = Resources.LoadAll("Portraits", typeof(Sprite));
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



        gameState.ShowIt();

        playerOneBlocksCollected = new bool[7];
        playerTwoBlocksCollected = new bool[7];

       if(gamesPlayedBeforeRefill == 0) { gamesPlayedBeforeRefill = 28; }

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

            return new Vector2(middleX * -1, bubbleManager_Ref.p2_LeftTop.y);
        }
        else
        {
            float topLeftX = bubbleManager_Ref.p2_LeftTop.x;
            float topRightX = bubbleManager_Ref.p2_RightTop.x;

            float middleX = Mathf.Abs((topLeftX - topRightX) / 2) + topLeftX;

            return new Vector2(middleX, bubbleManager_Ref.p2_LeftTop.y);
        }
    }

    public bool Collected(bool isPlayerOne, int blockCollected)
    {

        if (isPlayerOne)
        {
            playerOneBlocksCollected[blockCollected] = true;
            inGameUIManager_Ref.ActivateImage(true, blockCollected);
        }
        else
        {
            playerTwoBlocksCollected[blockCollected] = true;
            inGameUIManager_Ref.ActivateImage(false, blockCollected);
        }

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
        if (gamesPlayed > gamesPlayedBeforeRefill)
        {
            portrait_Refs = Resources.LoadAll("Portraits", typeof(Sprite));
            gamerTag_Refs = Resources.LoadAll("GamerTags", typeof(TextAsset));
            introPhrases = Resources.LoadAll("IntroPhrases", typeof(TextAsset));
            victoryPhrases = Resources.LoadAll("VictoryPhrases", typeof(TextAsset));

            gamesPlayed = -1;
        }

        playerOneCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases, gamerTag_Refs);
        playerTwoCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases, gamerTag_Refs);
        playerCharacterArray = new PlayerCharacter[] { playerOneCharacter, playerTwoCharacter };

        playerOneBlocksCollected = new bool[7];
        playerTwoBlocksCollected = new bool[7];
        gameOver = false;
        playerOneWon = false;
        introIsDone = false;
        inGameUIManager_Ref.ResetCollectedBlocks();
        inGameUIManager_Ref.UpdatePlayerDisplays();
        inGameText_Ref.text = "Get Ready!";
        RefillSpawners();

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
        if (blockPrefab_Refs.Length == 7)
        {
            for (int i = 0; i < 7; i++)
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
        }
        else
        {
            Debug.Log("blockPrefab_Refs does not have all the references it needs");
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

    // properties
    public GameObject Canvas_Ref {get {return canvas_Ref;}}
    public BubbleManager BubbleManager_Ref { set { bubbleManager_Ref = value; } get { return bubbleManager_Ref; } }
    public PlayerCharacter[] PlayerCharacterArray { get { return playerCharacterArray; } }
    public bool IntroIsDone { get { return introIsDone; } set { introIsDone = value; } }
    public bool PlayerOneWon { get { return playerOneWon; } }
    public bool GameOver { get { return gameOver; } }
   

}
