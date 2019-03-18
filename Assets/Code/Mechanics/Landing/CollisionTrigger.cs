using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionTrigger : MonoBehaviour
{
    ParticleSystem particleCrash;

    private GameStateMachine gameStateMachine_Ref;
    private bool amIPlayerOne;
    private Spawner mySpawner_Ref;

    [Header("Testing Player Input")]
    public PlayerInput playerInput;

    private float reSpawnCooldown;
    private float reSpawnCooldownTracker;

    void Start()
    {
       amIPlayerOne = AmIOnPlayerOneSide();
        gameStateMachine_Ref = GameStateMachine.GetInstance();

        if (amIPlayerOne)
        {
           mySpawner_Ref = gameStateMachine_Ref.playerOneSpawner_Ref.GetComponent<Spawner>();
        }
        else
        {
           mySpawner_Ref = gameStateMachine_Ref.playerTwoSpawner_Ref.GetComponent<Spawner>();
        }

        reSpawnCooldown = 0.1f;
    }

    void Update()
    {
        UpdateCooldownTracker();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (reSpawnCooldownTracker > reSpawnCooldown)
        {


            //GameObject testShape = GameObject.Find("TestShape");



            //if (collision.gameObject.tag == "block")
            //{
            GameObject blockShape = collision.gameObject;
            Quaternion blockRotation = blockShape.transform.rotation;
            //Debug.Log(blockRotation.eulerAngles.z);

            //float zValue = blockRotation.eulerAngles.z;

            if (collision.gameObject.tag == "block1")
            {

                //Debug.Log("Block Type 1");
                //GameObject blockOne = GameObject.FindGameObjectWithTag("block");
                //GameObject blockOne = GameObject.Find("TestPiece");


                if (blockRotation.eulerAngles.z < 2f || blockRotation.eulerAngles.z > 350f)
                {
                    //Debug.Log("You fit!");

                    //Gives the player scores.
                    //GameStateMachine.GetInstance().Collected(bool playerOne, int BlockOne);

                    EventManager.TriggerEvent("BlocksCollected"); //This event should only fire when all blocks are collected
                                                                  //GameObject.FindGameObjectWithTag("block");


                    //Remove the block from the spawner.

                    SpawnNextAndKillBlock(collision);

                    //Destroy(blockOne);
                    //Particle should fire, but this is best done from a script on the gameobject.

                }



                else
                {

                    //Destroy(blockOne);
                    SpawnNextAndKillBlock(collision);
                    //  Debug.Log("Destroyed!");
                }

            }




            else if (collision.gameObject.tag == "block2")
            {

                //Debug.Log("Block Type 2");
                //GameObject blockTwo = GameObject.FindGameObjectWithTag("block2");
                //Destroy(blockTwo);
                // Debug.Log("Destroyed!");
                SpawnNextAndKillBlock(collision);



            }

            else if (collision.gameObject.tag == "block3")
            {

                //Debug.Log("Block Type 3");
                //GameObject blockThree = GameObject.FindGameObjectWithTag("block3");
                //Destroy(blockThree);
                //  Debug.Log("Destroyed!");
                SpawnNextAndKillBlock(collision);



            }

            //}
            reSpawnCooldownTracker = 0.0f;
        }
    }
        private bool AmIOnPlayerOneSide()
        {
            if (transform.position.x < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void SpawnNextAndKillBlock(Collision2D collision)
        {

            //gameStateMachine_Ref.Collected(amIPlayerOne, (int)collision.gameObject.GetComponent<MyBlockType>().myBlockType);
            mySpawner_Ref.RemoveGroup();
            if (!gameStateMachine_Ref.GameOver)
            {
                if (!gameStateMachine_Ref.Collected(amIPlayerOne, (int)collision.gameObject.GetComponent<MyBlockType>().myBlockType))
                    mySpawner_Ref.SpawnNext();
            }
            Destroy(collision.gameObject);
        }
        private void UpdateCooldownTracker()
        {
            reSpawnCooldownTracker += Time.deltaTime;  
        }
    

}
