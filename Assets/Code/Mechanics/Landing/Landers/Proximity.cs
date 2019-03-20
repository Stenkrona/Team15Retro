using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity : MonoBehaviour
{
    SpriteRenderer greenGlowRenderer;
    GameObject targetPoint;
    float distanceToTarget;
    GameObject myBlock;
    GameObject myLander;

    public void Awake()
    {
        GameObject targetPoint = GameObject.Find("Target");
    }

    public void Start()
    {
       
        greenGlowRenderer = gameObject.GetComponent<SpriteRenderer>();
        //GameObject targetPoint = GameObject.Find("Target");

        //100% transparent
        greenGlowRenderer.color = new Color(1f, 1f, 1f, 0f);

    }


        void Update()
    {

        if (myBlock != null && myLander != null)
        {
            distanceToTarget = Vector2.Distance(myBlock.transform.position, myLander.transform.position);

            if (distanceToTarget < 3 && distanceToTarget > 1)
            {
                Debug.Log("You are close!");
                greenGlowRenderer.color = new Color(1f, 1f, 1f, .5f);
            }

            else if (distanceToTarget < 1)
            {
                Debug.Log("Wooooaaaah super close!!");
                greenGlowRenderer.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.GetComponent<MyBlockType>().myBlockType == BlockType.T)

    //    {
    //        //50% transparent
    //        greenGlowRenderer.color = new Color(1f, 1f, 1f, .5f);
    //        //greenGlowRenderer.color = new Color(1f,1f,1f,1f) <- normal
    //    }


    //}
}
