using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionTrigger : MonoBehaviour
{
    ParticleSystem particleCrash;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // EventManager.TriggerEvent("OnCollisionLander");

        //GameObject testShape = GameObject.Find("TestShape");

        
        //if (collision.gameObject.tag == "block")
        //{
            GameObject blockShape = collision.gameObject;
            Quaternion blockRotation = blockShape.transform.rotation;
            //Debug.Log(blockRotation.eulerAngles.z);

            //float zValue = blockRotation.eulerAngles.z;

        if (collision.gameObject.tag == "block")
        {

            Debug.Log("Block Type 1");
            GameObject blockOne = GameObject.Find("TestPiece");

            //Use the Operator on the other places as well.
            if (blockRotation.eulerAngles.z < 2f || blockRotation.eulerAngles.z > 350f)
            {
                Debug.Log("You fit!");
                EventManager.TriggerEvent("BlockCollected"); //This event should only fire when all blocks are collected
                GameObject.FindGameObjectWithTag("block");
                FindObjectOfType<Spawner>().SpawnNext();
                //The block should also be removed from the spawner array
                Destroy(blockOne);
                //Particle should fire, but this is best done from a script on the gameobject.
            }

            //else if (blockRotation.eulerAngles.z > 350f)
            //{
            //    Debug.Log("You fit event though you spun like crazy!");
            //    EventManager.TriggerEvent("BlockCollected");
            //}

        else
            {
                
                Destroy(blockOne);
                Debug.Log("Destroyed!");
            }

        }


        //Please don't look at the code down below, it's all wrong, I'm changing it tomorrow!

        else if (collision.gameObject.tag == "block2")
        {

            Debug.Log("Block Type 2");

            if (blockRotation.eulerAngles.z < 2f)
            {
                Debug.Log("You fit!");
                EventManager.TriggerEvent("BlockCollected");
            }

            else if (blockRotation.eulerAngles.z > 350f)
            {
                Debug.Log("You fit event though you spun like crazy!");
                EventManager.TriggerEvent("BlockCollected");
            }

        }

        else if (collision.gameObject.tag == "block3")
        {

            Debug.Log("Block Type 3");

            if (blockRotation.eulerAngles.z < 2f)
            {
                Debug.Log("You fit!");
                EventManager.TriggerEvent("BlockCollected");
            }

            else if (blockRotation.eulerAngles.z > 350f)
            {
                Debug.Log("You fit event though you spun like crazy!");
                EventManager.TriggerEvent("BlockCollected");
            }

        }

        //}

    }

}
