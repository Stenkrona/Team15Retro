using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BubbleManager : MonoBehaviour
{
    private static BubbleManager bubbleManager_Ref;
    private GameStateMachine gameStateMachine_Ref;

    [Header("Manager Settings")]
    public bool isOn;
    public float noSpawnUpperBuffer;
    public float noSpawnLowerBuffer;
    public int maxBubblesInPlay;
    public float minLengthTweenYValues;
    public bool alternateSpawningSide;

    [Header("Bubble Properties")]
    public GameObject bubblePrefab_Ref;
    public GameObject bubblesParent;
    public float bubbleMovementSpeed;
    public float bubbleElevationSpeed;

    [Header("Player One Borders")]
    public Vector2 p1_LeftBottom;
    public Vector2 p1_LeftTop;
    public Vector2 p1_RightBottom;
    public Vector2 p1_RightTop;

    [Header("Player Two Borders")]
    public Vector2 p2_LeftBottom;
    public Vector2 p2_LeftTop;
    public Vector2 p2_RightBottom;
    public Vector2 p2_RightTop;

    public Vector2[] borderVectorsArray;

    private List<GameObject> bubblesOnPlayerOnesSide;
    private List<GameObject> bubblesOnPlayerTwosSide;

    private bool isSpawningRightSide;
    private float deSpawnZone;
    private bool hasSpawnedLeftSide;

    public static BubbleManager GetInstance()
    {
        return bubbleManager_Ref;
    }
    
    void Awake()
    {
        if(bubbleManager_Ref == null)
        {
            bubbleManager_Ref = this;
        }
        else
        {
            Destroy(this);
        }
    }


    void Start()
    {
        gameStateMachine_Ref = GameStateMachine.GetInstance(); 

        MakeArrayOfVectors();

        GetBorders();
        SetVectors();
        CheckBorders();
      

        bubblesOnPlayerOnesSide = new List<GameObject>();
        bubblesOnPlayerTwosSide = new List<GameObject>();

        if (maxBubblesInPlay == 0) maxBubblesInPlay = 1;
        if (noSpawnUpperBuffer == 0) noSpawnUpperBuffer = 1;
        if (noSpawnLowerBuffer == 0) noSpawnLowerBuffer = 1;
        if (minLengthTweenYValues == 0) minLengthTweenYValues = 1;
       
    }

    
    void Update()
    {
        RunBubbleMechanic();
    }
    public void TurnOn()
    {
        isOn = true;
        ToggleBubbles(false);
    }
    public void TurnOff()
    {
        isOn = false;
        ToggleBubbles(true);
    }
    private void MakeArrayOfVectors()
    {
        borderVectorsArray = new Vector2[] {p1_LeftBottom, p1_LeftTop, p1_RightBottom, p1_RightTop,
            p2_LeftBottom, p2_LeftTop, p2_RightBottom, p2_RightTop};
    }
    
    private bool NullCheckBubblePrefab_Ref()
    {
        if(bubblePrefab_Ref != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void CheckBorders()
    {
        //Go through all border vectors to see that they have relevant values.

        foreach (Vector2 v in borderVectorsArray)
        {
            if (!CheckVector(v))
            {
                Debug.Log("A border vector is missing relevant values.");
            }
        }
    }
    private bool CheckVector(Vector2 v)
    {
        //Check if the vector2 has relevant values and not just 0 and 0;

        if (v.x == 0.0f && v.y == 0.0f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void RunBubbleMechanic()
    {
        if (isOn)
        {
            if(bubblesOnPlayerOnesSide.Count < maxBubblesInPlay)
            {
                //Spawn on player one side.

               

                int timesToSpawn = Mathf.Abs(bubblesOnPlayerOnesSide.Count - maxBubblesInPlay);

                for (int i = 0; i < timesToSpawn ; i++)
                {
                    SpawnBubble(GetRandomSpawnPosition(true), true);
                }
               
            }
            
            if(bubblesOnPlayerTwosSide.Count < maxBubblesInPlay)
            {
                //Spawn on player two side.

                int timesToSpawn = Mathf.Abs(bubblesOnPlayerTwosSide.Count - maxBubblesInPlay);

                for (int i = 0; i < timesToSpawn; i++)
                {
                    SpawnBubble(GetRandomSpawnPosition(false), false);
                }
               
            }
        }
        
    }
    private void SpawnBubble(Vector2 v, bool playerOneSide)
    {
        if (NullCheckBubblePrefab_Ref())
        {
            
            GameObject tempBubbleGameObjcet = Instantiate(bubblePrefab_Ref, CheckYValue(ClampVector(v), playerOneSide), Quaternion.identity);
           
            Bubble tempBubble_Ref = tempBubbleGameObjcet.GetComponent<Bubble>();

            //Putting the bubbles under the same parent to avoid messiness.
            tempBubbleGameObjcet.transform.SetParent(bubblesParent.transform);
            if (isSpawningRightSide) tempBubble_Ref.isMovingLeft = true;
            tempBubble_Ref.myDeSpawnValue = deSpawnZone;
            tempBubble_Ref.bubbleManager_Ref = this;
            tempBubble_Ref.gameStateMachine_Ref = gameStateMachine_Ref;
            tempBubble_Ref.movementSpeed = bubbleMovementSpeed;
            tempBubble_Ref.elevationSpeed = bubbleElevationSpeed;

            if (playerOneSide)
            {
                bubblesOnPlayerOnesSide.Add(tempBubbleGameObjcet);
                tempBubble_Ref.isOnPlayerOneSide = true;


            }
            else
            {
                bubblesOnPlayerTwosSide.Add(tempBubbleGameObjcet);
            }
        }
        else
        {
            Debug.Log("BubbleManager is missing a reference to BubblePrefab");
        }
    }
    private Vector2 GetRandomSpawnPosition(bool playerOneSide)
    {
       
        if (playerOneSide)
        {
            //Spawn on the Player One side.


            float randomFloat = Random.Range(0.0f, 1.0f);

            if(randomFloat > .5 && !alternateSpawningSide || !hasSpawnedLeftSide && alternateSpawningSide)
            {
                //Left side
                float randomYValue = Random.Range(p1_LeftBottom.y, p1_LeftTop.y - noSpawnUpperBuffer);

                isSpawningRightSide = false;
                deSpawnZone = p1_RightBottom.x;

                Vector2 vectorToReturn = new Vector2(p1_LeftTop.x, randomYValue);
                if (alternateSpawningSide) hasSpawnedLeftSide = true;


                return new Vector2(p1_LeftTop.x, randomYValue);
              
            }
            else if(randomFloat < .5 && !alternateSpawningSide || hasSpawnedLeftSide && alternateSpawningSide)
            {
                //Right side
                float randomYValue = Random.Range(p1_RightBottom.y, p1_RightTop.y - noSpawnUpperBuffer);

                isSpawningRightSide = true;
                deSpawnZone = p1_LeftBottom.x;

                if (alternateSpawningSide) hasSpawnedLeftSide = false;

                return new Vector2(p1_RightTop.x, randomYValue);
            }
            else
            {
                return new Vector2();
            }

            
        }
        else
        {
            //Spawn on Player Two side

            float randomFloat = Random.Range(0.0f, 1.0f);

            if(randomFloat > .5 && !alternateSpawningSide || !hasSpawnedLeftSide && alternateSpawningSide)
            {
                //Left side
                float randomYValue = Random.Range(p2_LeftBottom.y, p2_LeftTop.y - noSpawnUpperBuffer);

                isSpawningRightSide = false;
                deSpawnZone = p2_RightBottom.x;

                if (alternateSpawningSide) hasSpawnedLeftSide = true;

                return new Vector2(p2_LeftBottom.x, randomYValue);
            }
            else if(randomFloat < .5 && !alternateSpawningSide || hasSpawnedLeftSide && alternateSpawningSide)
            {
                //Right Side
                float randomYValue = Random.Range(p2_RightBottom.y, p2_RightTop.y - noSpawnUpperBuffer);

                isSpawningRightSide = true;
                deSpawnZone = p2_LeftBottom.x;

                if (alternateSpawningSide) hasSpawnedLeftSide = false;

                return new Vector2(p2_RightBottom.x, randomYValue);
            }
            else
            {
                return new Vector2();
            }
        }
    }
    private Vector2 ClampVector(Vector2 v)
    {
        float xValue = v.x;
        float yValue = v.y;

        if (v.y < p1_LeftBottom.y + noSpawnLowerBuffer)
        {
            yValue = p1_LeftBottom.y + noSpawnLowerBuffer;
            return new Vector2(xValue, yValue);
        }
        else
        {
            return v;
        }
    }
    private void ToggleBubbles(bool b)
    {
        foreach(GameObject go in bubblesOnPlayerOnesSide)
        {
            go.GetComponent<Bubble>().isPaused = b;
        }
        foreach(GameObject go in bubblesOnPlayerTwosSide)
        {
            go.GetComponent<Bubble>().isPaused = b;
        }
    }
    private Vector2 CheckYValue(Vector2 v, bool playerOneSide)
    {
        float xValue = v.x;

        if (playerOneSide)
        {
            if (bubblesOnPlayerOnesSide.Count > 0)
            {
                float distanceValue = v.y - bubblesOnPlayerOnesSide[0].transform.position.y;

                if (Mathf.Abs(distanceValue) < minLengthTweenYValues)
                {
                    float randomYvalue = Random.Range(p1_LeftBottom.y, p1_LeftTop.y);

                    Vector2 newTry = new Vector2(xValue, randomYvalue);

                    return CheckYValue(newTry, playerOneSide);
                }
                else
                {
                    return v;
                }
            }
            else
            {
                return v;
            }
        }
        else
        {
            if (bubblesOnPlayerTwosSide.Count > 0)
            {
                float distanceValue = v.y - bubblesOnPlayerTwosSide[0].transform.position.y;

                if (Mathf.Abs(distanceValue) < minLengthTweenYValues)
                {
                    float randomYvalue = Random.Range(p2_LeftBottom.y, p2_LeftTop.y);

                    Vector2 newTry = new Vector2(xValue, randomYvalue);

                    return CheckYValue(newTry, playerOneSide);
                }
                else
                {
                    return v;
                }
            }
            else
            {
                return v;
            }
        }
        

    }
    private void GetBorders()
    {
        GameObject borderParent_Ref = gameStateMachine_Ref.bordersParent_Ref;

        int childAmount = borderParent_Ref.transform.childCount;

        

        for (int i = 0; i < childAmount; i++)
        {
            Vector2 temp = borderParent_Ref.transform.GetChild(i).transform.position;
            borderVectorsArray[i] = temp;
        }
    }
    private void SetVectors()
    {
        p1_LeftBottom = borderVectorsArray[0];
        p1_LeftTop = borderVectorsArray[1];
        p1_RightBottom = borderVectorsArray[2];
        p1_RightTop = borderVectorsArray[3];
        p2_LeftBottom = borderVectorsArray[4];
        p2_LeftTop = borderVectorsArray[5];
        p2_RightBottom = borderVectorsArray[6];
        p2_RightTop = borderVectorsArray[7];
    }
    public void KillAllBubbles()
    {
        foreach(GameObject bubble in bubblesOnPlayerOnesSide)
        {
            Destroy(bubble);
        }
        foreach (GameObject bubble in bubblesOnPlayerTwosSide)
        {
            Destroy(bubble);
        }

        bubblesOnPlayerOnesSide = new List<GameObject>();
        bubblesOnPlayerTwosSide = new List<GameObject>();

    }
    
    //properties
    public List<GameObject> BubblesOnPlayerOnesSide
    { get { return bubblesOnPlayerOnesSide; }
        set { bubblesOnPlayerOnesSide = value; }}
    public List<GameObject> BubblesOnPlayerTwosSide
    { get { return bubblesOnPlayerTwosSide; }
        set { bubblesOnPlayerTwosSide = value; } }


}
