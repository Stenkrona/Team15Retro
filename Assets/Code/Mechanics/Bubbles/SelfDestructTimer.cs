using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructTimer : MonoBehaviour
{
    
    void Start()
    {
        Invoke("DestroySelf", 1.0f);
    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
   
}
