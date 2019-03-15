using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Interfaces;
using UnityEngine.UI;

public class GameStateMachine : MonoBehaviour
{
    public bool debugMode;
    public GameObject canvas_Ref;
    public GameObject bordersParent_Ref;
    public GameObject playerOneParent_Ref;
    public GameObject playerTwoParent_Ref;
    public GameObject playerOneSpawner_Ref;
    public GameObject playerTwoSpawner_Ref;
    public BubbleManager bubbleManager_Ref;

    public float timeToWaitBeforeScoreScreen;

    private IStateBase gameState;
    

    private static GameStateMachine gameStateMachine_Ref;
   
    private Text debugTxtReference;
    private string currentState;

    private bool[] playerOneBlocksCollected;
    private bool[] playerTwoBlocksCollected;

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

    public void Collected(bool isPlayerOne, int blockCollected)
    {
        if (isPlayerOne)
        {
            playerOneBlocksCollected[blockCollected] = true;
        }
        else
        {
            playerTwoBlocksCollected[blockCollected] = true;
        }

        CheckIfSomeoneWon();
    }
    private void CheckIfSomeoneWon()
    {
        bool playerOneWon = true;
        bool playerTwoWon = true;

        foreach(bool b in playerOneBlocksCollected)
        {
            if (!b) playerOneWon = false; break;
        }

        foreach(bool b in playerTwoBlocksCollected)
        {
            if (!b) playerTwoWon = false; break;
        }

        if (playerOneWon) WeHaveAWinner(true);
        else if (playerTwoWon) WeHaveAWinner(false);
    }
    private void WeHaveAWinner(bool isPlayerOne)
    {
        Invoke("ChangeToScoreScreen", timeToWaitBeforeScoreScreen);
    }
    private void ChangeToScoreScreen()
    {
        ChangeState(new ScoreScreenState(this));
    }
   
  
   
    // properties
    public GameObject Canvas_Ref {get {return canvas_Ref;}}
    public BubbleManager BubbleManager_Ref { set { bubbleManager_Ref = value; } get { return bubbleManager_Ref; } }
  
   

}
