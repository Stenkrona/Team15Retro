using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockArray : MonoBehaviour
{
    [Header("GameObjects in array")]
    public GameObject[] blocks;


    void Start()
    {
        blocks = GameObject.FindGameObjectsWithTag("block");

        for (int i = 0; i <blocks.Length; i++)
        {
            Debug.Log("Block " + i + " is named " + blocks[i].name);
        }
    }


}
