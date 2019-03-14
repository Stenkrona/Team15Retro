using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [HideInInspector] public bool isMovingLeft;
    [HideInInspector] public float movementSpeed;
    [HideInInspector] public bool isOnPlayerOneSide;
    [HideInInspector] public float myDeSpawnValue;
    [HideInInspector] public bool isPaused;

    [HideInInspector] public BubbleManager bubbleManager_Ref;
    [HideInInspector] public float elevationSpeed;

    private bool hasCaughtTetrisBlock;

    void Start()
    {
        if (movementSpeed == 0) movementSpeed = 5;
        if (elevationSpeed == 0) elevationSpeed = 3;
    }

    void Update()
    {
        Move();
        CheckDeSpawn();
    }

    void Move()
    {
        if (!isPaused)
        {
            if (!hasCaughtTetrisBlock)
            {
                if (isMovingLeft)
                {
                    transform.Translate(Vector2.left * Time.deltaTime * movementSpeed);
                }
                else
                {
                    transform.Translate(Vector2.right * Time.deltaTime * movementSpeed);
                }
            }
            else
            {
                transform.Translate(Vector2.up * Time.deltaTime * elevationSpeed);

                if(transform.position.y >= bubbleManager_Ref.noSpawnUpperBuffer)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void CheckDeSpawn()
    {
        if (isMovingLeft && transform.position.x < myDeSpawnValue)
        {
            RemoveMeFromBubbleManagerList();
            Destroy(gameObject);
        }
        else if(!isMovingLeft && transform.position.x > myDeSpawnValue)
        {
            RemoveMeFromBubbleManagerList();
            Destroy(gameObject);
        }
    }
    private void RemoveMeFromBubbleManagerList()
    {
        if (isOnPlayerOneSide)
        {
            bubbleManager_Ref.BubblesOnPlayerOnesSide.Remove(gameObject);
        }
        else
        {
            bubbleManager_Ref.BubblesOnPlayerTwosSide.Remove(gameObject);
        }
    }
    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            hasCaughtTetrisBlock = true;
        }
    }
}
