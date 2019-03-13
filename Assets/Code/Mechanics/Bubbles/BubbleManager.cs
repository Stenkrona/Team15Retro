﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BubbleManager : MonoBehaviour
{
    private GameStateMachine gameStateMachine_Ref;

    public bool isOn;
    public float noSpawnUpperBuffer;
    public float noSpawnLowerBuffer;

    public GameObject bubblePrefab_Ref;
    public GameObject bubblesParent;
    public int maxBubblesInPlay;
    public float minLengthTweenYValues;

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

    private Vector2[] borderVectorsArray;

    private List<GameObject> bubblesOnPlayerOnesSide;
    private List<GameObject> bubblesOnPlayerTwosSide;

    private bool isSpawningRightSide;
    private float deSpawnZone;
    


    void Start()
    {
        gameStateMachine_Ref = Camera.main.GetComponent<GameStateMachine>();

        if (NullCheckGameStateMachine())
        {
            gameStateMachine_Ref.BubbleManager_Ref = this;
        }
        else
        {
            Debug.Log("BubbleManager is missing a reference to GameStateMachine");
        }
        if (!NullCheckBubblePrefab_Ref())
        {
            Debug.Log("BubbleManager is missing a reference to BubblePrefab");
        }
        MakeArrayOfVectors();

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
    private bool NullCheckGameStateMachine()
    {
        if(gameStateMachine_Ref == null)
        {
            return false;
        }
        else
        {
            return true;
        }
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

                for (int i = 0; i < Mathf.Abs(bubblesOnPlayerOnesSide.Count - maxBubblesInPlay) ; i++)
                {
                    SpawnBubble(GetRandomSpawnPosition(true), true);
                }
               
            }
            
            if(bubblesOnPlayerTwosSide.Count < maxBubblesInPlay)
            {
                //Spawn on player two side.

                for (int i = 0; i < Mathf.Abs(bubblesOnPlayerTwosSide.Count - maxBubblesInPlay); i++)
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

            if(randomFloat > .5)
            {
                //Left side
                float randomYValue = Random.Range(p1_LeftBottom.y, p1_LeftTop.y - noSpawnUpperBuffer);

                isSpawningRightSide = false;
                deSpawnZone = p1_RightBottom.x;

                Vector2 vectorToReturn = new Vector2(p1_LeftTop.x, randomYValue);

              
                return new Vector2(p1_LeftTop.x, randomYValue);
              
            }
            else
            {
                //Right side
                float randomYValue = Random.Range(p1_RightBottom.y, p1_RightTop.y - noSpawnUpperBuffer);

                isSpawningRightSide = true;
                deSpawnZone = p1_LeftBottom.x;

                return new Vector2(p1_RightTop.x, randomYValue);
            }
        }
        else
        {
            //Spawn on Player Two side

            float randomFloat = Random.Range(0.0f, 1.0f);

            if(randomFloat > .5)
            {
                //Left side
                float randomYValue = Random.Range(p2_LeftBottom.y, p2_LeftTop.y - noSpawnUpperBuffer);

                isSpawningRightSide = false;
                deSpawnZone = p2_RightBottom.x;

                return new Vector2(p2_LeftBottom.x, randomYValue);
            }
            else
            {
                //Right Side
                float randomYValue = Random.Range(p2_RightBottom.y, p2_RightTop.y - noSpawnUpperBuffer);

                isSpawningRightSide = true;
                deSpawnZone = p2_LeftBottom.x;

                return new Vector2(p2_RightBottom.x, randomYValue);
            }
        }
    }
    private Vector2 ClampVector(Vector2 v)
    {
        float xValue = v.x;
        float yValue = v.y;

        if (v.y < noSpawnLowerBuffer)
        {
            yValue = noSpawnLowerBuffer;
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
    //properties
    public List<GameObject> BubblesOnPlayerOnesSide
    { get { return bubblesOnPlayerOnesSide; }
        set { bubblesOnPlayerOnesSide = value; }}

    public List<GameObject> BubblesOnPlayerTwosSide
    { get { return bubblesOnPlayerTwosSide; }
        set { bubblesOnPlayerTwosSide = value; } }


}