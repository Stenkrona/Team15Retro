using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity : MonoBehaviour
{
    SpriteRenderer greenGlowRenderer;

    public void Start()
    {
       
        greenGlowRenderer = gameObject.GetComponent<SpriteRenderer>();

        //100% transparent
        greenGlowRenderer.color = new Color(1f, 1f, 1f, 0f);

        //GameObject blockTypeL = gameObject.GetComponent<MyBlockType>;
        GameObject blockT = GameObject.FindGameObjectWithTag("blockT");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MyBlockType>().myBlockType == BlockType.T)

        {
            //50% transparent
            greenGlowRenderer.color = new Color(1f, 1f, 1f, .5f);
            //greenGlowRenderer.color = new Color(1f,1f,1f,1f) <- normal
        }

       
    }



        private void Update()
    {
        
    }

}
