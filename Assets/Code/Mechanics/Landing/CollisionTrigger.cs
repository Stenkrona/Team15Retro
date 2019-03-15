using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionTrigger : MonoBehaviour
{
    ParticleSystem particleCrash;

    [Header("Testing Player Input")]
    public PlayerInput playerInput;


    private void OnCollisionEnter2D(Collision2D collision)
    {

        //GameObject testShape = GameObject.Find("TestShape");

        //collision.gameObject.GetComponent<PlayerController>().input.name == "PlayerOneInput";

        //if (collision.gameObject.tag == "block")
        //{
            GameObject blockShape = collision.gameObject;
            Quaternion blockRotation = blockShape.transform.rotation;
            //Debug.Log(blockRotation.eulerAngles.z);

            //float zValue = blockRotation.eulerAngles.z;

        if (collision.gameObject.tag == "block")
        {

            Debug.Log("Block Type 1");
            //GameObject blockOne = GameObject.FindGameObjectWithTag("block");
            //GameObject blockOne = GameObject.Find("TestPiece");


            if (blockRotation.eulerAngles.z < 2f || blockRotation.eulerAngles.z > 350f)
            {
                Debug.Log("You fit!");

                //Gives the player scores.
                //GameStateMachine.GetInstance().Collected(bool playerOne, int BlockOne);

                EventManager.TriggerEvent("BlocksCollected"); //This event should only fire when all blocks are collected
                //GameObject.FindGameObjectWithTag("block");


                //Remove the block from the spawner.
                FindObjectOfType<Spawner>().SpawnNext();
                Destroy(gameObject);

                //Destroy(blockOne);
                //Particle should fire, but this is best done from a script on the gameobject.

            }



        else
            {

                FindObjectOfType<Spawner>().SpawnNext();
                //Destroy(blockOne);
                Destroy(gameObject);
                Debug.Log("Destroyed!");
            }

        }


        

        else if (collision.gameObject.tag == "block2")
        {

            Debug.Log("Block Type 2");
            //GameObject blockTwo = GameObject.FindGameObjectWithTag("block2");
            //Destroy(blockTwo);
            Debug.Log("Destroyed!");
            Destroy(gameObject);
            FindObjectOfType<Spawner>().SpawnNext();



        }

        else if (collision.gameObject.tag == "block3")
        {

            Debug.Log("Block Type 3");
            //GameObject blockThree = GameObject.FindGameObjectWithTag("block3");
            //Destroy(blockThree);
            Debug.Log("Destroyed!");
            Destroy(gameObject);
            FindObjectOfType<Spawner>().SpawnNext();



        }

        //}

    }

}
