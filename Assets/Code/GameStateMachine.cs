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

    [Header("Settings")]
    public bool debugMode;
    public float timeToWaitBeforeScoreScreen;

    private IStateBase gameState;
    

    private static GameStateMachine gameStateMachine_Ref;
   
    private Text debugTxtReference;
    private string currentState;

    private PlayerCharacter playerOneCharacter;
    private PlayerCharacter playerTwoCharacter;
    private PlayerCharacter[] playerCharacterArray;

    private Object[] portrait_Refs;
    private Object[] introPhrases;
    private Object[] victoryPhrases;
   


    private bool[] playerOneBlocksCollected;
    private bool[] playerTwoBlocksCollected;
    private bool introIsDone;
    private bool gameOver;
    private bool playerOneWon;

    public static GameStateMachine GetInstance()
    {
        return gameStateMachine_Ref;
    }

    void Awake()
    {
        if(gameStateMachine_Ref == null)
        {
            gameStateMachine_Ref = this;
        }
        else
        {
            Destroy(this);
        }

        portrait_Refs = Resources.LoadAll("Portraits", typeof(Sprite));
        introPhrases = Resources.LoadAll("IntroPhrases", typeof(TextAsset));
        victoryPhrases = Resources.LoadAll("VictoryPhrases", typeof(TextAsset));

        playerOneCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases);
        playerTwoCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases);
        playerCharacterArray = new PlayerCharacter[] { playerOneCharacter, playerTwoCharacter };

      
    }
    void Start()
    {
        gameStateMachine_Ref = this;

         if(canvas_Ref == null)
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
         

        if(gameState == null)
        {
            gameState = new BeginState(this);
        }
       
        if(bordersParent_Ref == null)
        {
            if(GameObject.Find("BordersParent") != null)
            {
                bordersParent_Ref = GameObject.Find("BordersParent");
            }
            else
            {
                Debug.Log("GameStateMachine is missing a reference to BordersParent");
            }
        }

        if(playerInfoOne_Ref == null)
                Debug.Log("GameStateMachine is missing a reference to playerOneInfo");
            
        
        if (playerInfoTwo_Ref == null)
                Debug.Log("GameStateMachine is missing a reference to playerTwoInfo");
            
        

        gameState.ShowIt();

        playerOneBlocksCollected = new bool[7];
        playerTwoBlocksCollected = new bool[7];

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

            float middleX = Mathf.Abs((topLeftX - topRightX)/2) - topRightX;

            return new Vector2(middleX * -1, bubbleManager_Ref.p2_LeftTop.y);
        }
        else
        {
            float topLeftX = bubbleManager_Ref.p2_LeftTop.x;
            float topRightX = bubbleManager_Ref.p2_RightTop.x;

            float middleX = Mathf.Abs((topLeftX - topRightX) /2) + topLeftX;

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

        foreach(bool b in playerOneBlocksCollected)
        {
            if (!b)
            {
                
                playerOneWon = false; break;
            }
        }

        foreach(bool b in playerTwoBlocksCollected)
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
        if (gameStateMachine_Ref == null)
        {
            gameStateMachine_Ref = this;
        }
        else
        {
            Destroy(this);
        }

        portrait_Refs = Resources.LoadAll("Portraits", typeof(Sprite));
        introPhrases = Resources.LoadAll("IntroPhrases", typeof(TextAsset));
        victoryPhrases = Resources.LoadAll("VictoryPhrases", typeof(TextAsset));

        playerOneCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases);
        playerTwoCharacter = new PlayerCharacter(portrait_Refs, introPhrases, victoryPhrases);
        playerCharacterArray = new PlayerCharacter[] { playerOneCharacter, playerTwoCharacter };

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
        gameOver = false;

    }



    // properties
    public GameObject Canvas_Ref {get {return canvas_Ref;}}
    public BubbleManager BubbleManager_Ref { set { bubbleManager_Ref = value; } get { return bubbleManager_Ref; } }
    public PlayerCharacter[] PlayerCharacterArray { get { return playerCharacterArray; } }
    public bool IntroIsDone { get { return introIsDone; } set { introIsDone = value; } }
    public bool PlayerOneWon { get { return playerOneWon; } }
    public bool GameOver { get { return gameOver; } }
   

}
