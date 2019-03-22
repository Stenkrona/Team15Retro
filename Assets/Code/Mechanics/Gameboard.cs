using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameboard : MonoBehaviour
{
    private static Gameboard gameboard_Ref;
    private GameStateMachine gameStateMachine_Ref;


    private List<TestLander> playerOneLanders;
    private List<TestLander> playerTwoLanders;
   

    [HideInInspector] public GameObject playerOneBlock_Ref;
    [HideInInspector] public GameObject playerTwoBlock_Ref;
    public Color playerOneOutlineColor;
    public Color playerTwoOutlineColor;

    private TestLander playerOneLanderTracked;
    private int playerOneLanderTrackedIndexValue;

    private TestLander playerTwoLanderTracked;
    private int playerTwoLanderTrackedIndexValue;

    private float playerOneDistance;
    private float playerTwoDistance;

    private Spawner playerOneSpawner;
    private Spawner playerTwoSpawner;

    private float[] playerOneFullDistances;
    private float[] playerTwoFullDistances;

    private Vector2 spawnerOneSpawnPosition;
    private Vector2 spawnerTwoSpawnPosition;

    

    void Awake()
    {
        if(gameboard_Ref == null)
        {
            gameboard_Ref = this;
        }
        else
        {
            Destroy(this);
        }

    }

    
    void Start()
    {

        gameStateMachine_Ref = GameStateMachine.GetInstance();
        playerOneFullDistances = new float[3];
        playerTwoFullDistances = new float[3];
        playerOneLanders = new List<TestLander>();
        playerTwoLanders = new List<TestLander>();
     

      //  GetAndSortLanders();
       
      
    }

    public static Gameboard GetInstance()
    {
        return gameboard_Ref;
    }
    void Update()
    {
        if (gameStateMachine_Ref.IntroIsDone)
        {


            GetDistances();

            playerOneOutlineColor = SetAlpha(true);
            playerTwoOutlineColor = SetAlpha(false);

            if (playerOneLanderTracked != null)
            {
                playerOneLanderTracked.transform.GetChild(1).GetComponent<SpriteRenderer>().color =
                    playerOneOutlineColor;
            }
            if(playerTwoLanderTracked != null)
            {
                playerTwoLanderTracked.transform.GetChild(1).GetComponent<SpriteRenderer>().color =
                    playerTwoOutlineColor;
            }
        }
      
    }


    public void AddLanderToList(bool playerOne, TestLander lander)
    {
        if (playerOne)
        {
            playerOneLanders.Add(lander);
        }
        else
        {
            playerTwoLanders.Add(lander);
        }
    }


    
    public void GetAndSortLanders()
    {
        if (GameObject.FindObjectsOfType<TestLander>() != null)
        {


            TestLander[] landers = GameObject.FindObjectsOfType<TestLander>();

            foreach (TestLander lander in landers)
            {
                if (lander.transform.position.x < 0)
                {
                    playerOneLanders.Add(lander);
                }
                else if (lander.transform.position.x > 0)
                {
                    playerTwoLanders.Add(lander);
                }
            }
        }
    }

    public void PairLanderWithObject(bool playerOne, GameObject block)
    {
        MyBlockType blockType = block.GetComponent<MyBlockType>();

        int tempIndex = 0;

        if (playerOne)
        {
            playerOneBlock_Ref = block;

            

            foreach(TestLander lander in playerOneLanders)
            {
                if(lander.blockTypeIWant == blockType.myBlockType)
                {
                    playerOneLanderTracked = lander;
                    playerOneLanderTrackedIndexValue = tempIndex;

                    break;
                }

                tempIndex++;
            }
        }
        else
        {
            playerTwoBlock_Ref = block;

            foreach(TestLander lander in playerTwoLanders)
            {
                if(lander.blockTypeIWant == blockType.myBlockType)
                {
                    playerTwoLanderTracked = lander;

                    playerTwoLanderTrackedIndexValue = tempIndex;

                    break;
                }

                tempIndex++;
            }
        }

        TurnOffIrrelevantOutlines();
    }

    private void GetDistances()
    {
        if(playerOneBlock_Ref != null && playerOneLanderTracked != null)
        {
            playerOneDistance = Vector2.Distance(playerOneBlock_Ref.transform.position,
                playerOneLanderTracked.transform.position);
        }
        
        if(playerTwoBlock_Ref != null && playerTwoLanderTracked != null)
        {
            playerTwoDistance = Vector2.Distance(playerTwoBlock_Ref.transform.position,
                playerTwoLanderTracked.transform.position);
        }
    }
    private void GetAndSortSpawners()
    {
        if (GameObject.FindObjectsOfType<Spawner>() != null)
        {

            Spawner[] spawners = GameObject.FindObjectsOfType<Spawner>();

            foreach (Spawner s in spawners)
            {
                if (s.playerSpawnpoint.x < 0)
                {
                    playerOneSpawner = s;
                }
                else
                {
                    playerTwoSpawner = s;
                }
            }
        }

    }

    public void SetFullDistances()
    {
      

        playerOneFullDistances[0] = Mathf.Abs(Vector2.Distance(
            playerOneLanders[0].transform.position, spawnerOneSpawnPosition));
        playerOneFullDistances[1] = Mathf.Abs(Vector2.Distance(
            playerOneLanders[1].transform.position, spawnerOneSpawnPosition));
        playerOneFullDistances[2] = Mathf.Abs(Vector2.Distance(
            playerOneLanders[2].transform.position, spawnerOneSpawnPosition));

        playerTwoFullDistances[0] = Mathf.Abs(Vector2.Distance(
            playerTwoLanders[0].transform.position, spawnerTwoSpawnPosition));
        playerTwoFullDistances[1] = Mathf.Abs(Vector2.Distance(
            playerTwoLanders[1].transform.position, spawnerTwoSpawnPosition));
        playerTwoFullDistances[2] = Mathf.Abs(Vector2.Distance(
            playerTwoLanders[2].transform.position, spawnerTwoSpawnPosition));

    }

    private float GetPercentageOfDistance(float currentDistance, float fullDistance)
    {
        return currentDistance / fullDistance;
    }
    private Color SetAlpha(bool playerOne)
    {
        if (playerOne)
        {
            float alpha = GetPercentageOfDistance(
                playerOneDistance, playerOneFullDistances[playerOneLanderTrackedIndexValue]);

            //Debug.Log(Mathf.Pow(2, 1 - alpha) - 1);

            return new Color(1, 1, 1, (Mathf.Pow(2, 1 - alpha) - 1));
        }
        else
        {
            float alpha = GetPercentageOfDistance(
                playerTwoDistance, playerTwoFullDistances[playerTwoLanderTrackedIndexValue]);

            return new Color(1, 1, 1, (Mathf.Pow(2, 1- alpha) - 1));
        }
    }
    private void TurnOffIrrelevantOutlines()
    {
        foreach(TestLander lander in playerOneLanders)
        {
            if(lander != playerOneLanderTracked)
            {
                lander.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            }
        }
        foreach(TestLander lander in playerTwoLanders)
        {
            if(lander != playerTwoLanderTracked)
            {
                lander.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            }
        }
    }
   
    
  

    //Properties
    public Spawner PlayerOneSpawner { set { playerOneSpawner = value; } }
    public Spawner PlayerTwoSpawner { set { playerTwoSpawner = value; } }

    public Vector2 SpawnerOneSpawnPosition { set { spawnerOneSpawnPosition = value; } }
    public Vector2 SpawnerTwoSpawnPosition { set { spawnerTwoSpawnPosition = value; } }
}
