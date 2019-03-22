using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLander : MonoBehaviour
{

    ParticleSystem particleCrash;

    public BlockType blockTypeIWant;

    private GameStateMachine gameStateMachine_Ref;
    private bool amIPlayerOne;
    private Spawner mySpawner_Ref;

    [Header("Testing Player Input")]
    public PlayerInput playerInput;

    private float reSpawnCooldown;
    private float reSpawnCooldownTracker;

    GameObject thisLander;

    private void Awake()
    {
        GameObject tBlock = GameObject.Find("T_Block");
        GameObject lBlock = GameObject.Find("L_Block");
        GameObject iblock = GameObject.Find("I_Block");
        thisLander = this.gameObject;
        
    }



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
            GameObject blockShape = collision.gameObject;
            Quaternion blockRotation = blockShape.transform.rotation;
            //Debug.Log(blockRotation.eulerAngles.z);

            //float zValue = blockRotation.eulerAngles.z;

            if (collision.gameObject.GetComponent<MyBlockType>().myBlockType == blockTypeIWant)
            {
                if (blockRotation.eulerAngles.z < 10f || blockRotation.eulerAngles.z > 350f && blockShape.GetComponent<PlayerController>().blockSpeed < 0.5f)
                {
                    Debug.Log("Win!");
                    SpawnNextAndKillBlock(collision);

                }
                else if (blockShape.GetComponent<PlayerController>().blockSpeed > 0.5f)
                {
                    Debug.Log("Too fast!");
                }

                else
                {


                    Debug.Log("Right block, wrong rotation!");
                    RespawnBlock(collision);
                }

            }


            else
            {
                //Debug.Log(collision.gameObject.name);
                Debug.Log("Wrong block!");
                //var knockBack = 500;
                //var knockBackForce = transform.position - thisLander.transform.position;

                //knockBackForce.Normalize();
                //blockShape.GetComponent<Rigidbody2D>().AddForce(knockBackForce * knockBack);

                //RespawnBlock(collision);
            }
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


    public void RespawnBlock (Collision2D collision)
    {
        Destroy(collision.gameObject);
        mySpawner_Ref.SpawnNext();
    }


    private void SpawnNextAndKillBlock(Collision2D collision)
    {
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
