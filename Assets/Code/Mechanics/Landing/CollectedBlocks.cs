using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlockOneCollected : MonoBehaviour
{
    private UnityAction blockListener;

    void Awake()
    {
        blockListener = new UnityAction(CollectBlocks);
    }

   
    private void OnEnable()
    {
        EventManager.StartListening("BlocksCollected", blockListener);

    }

    private void OnDisable()
    {
        EventManager.StopListening("BlocksCollected", blockListener);

    }

    void CollectBlocks()
    {
        Debug.Log("I collected all blocks!");


    }

   
}
