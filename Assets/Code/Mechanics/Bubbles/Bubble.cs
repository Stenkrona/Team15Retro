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

    void Start()
    {
        if (movementSpeed == 0) movementSpeed = 5;
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
            if (isMovingLeft)
            {
                transform.Translate(Vector2.left * Time.deltaTime * movementSpeed);
            }
            else
            {
                transform.Translate(Vector2.right * Time.deltaTime * movementSpeed);
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
}
