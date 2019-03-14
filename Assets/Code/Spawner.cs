using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject[] Groups;

    // Start is called before the first frame update
    void Start()
    {
        SpawnNext();
    }

    //call this when you want to spawn a random group
    //can be called using FindObjectOfType<Spawner>().SpawnNext(); OBS only works if the gameobject is called Spawner
    public void SpawnNext()
    {
        int I = Random.Range(0, Groups.Length);
        
        if(Groups[I] == null)
        {
            SpawnNext();
            return;
        }

        //Spawn group at current position
        Instantiate(Groups[I], transform.position, Quaternion.identity);

        Groups[I] = null;
    }

}
