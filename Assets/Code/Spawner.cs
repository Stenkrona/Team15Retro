using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Vector3 playerSpawnpoint;
    public PlayerInput playerInput;

    public float thrustPower;

    public GameObject[] groups;
    GameObject instanceGroup;


    // Start is called before the first frame update
    void Start()
    {
        SpawnNext();
    }

    public void RemoveGroup()
    {
        for(int i = 0; i<groups.Length;i++ )
        {
            if(instanceGroup == groups[i])
            {
                groups[i] = null;
            }
        }
    }
    
    //can be called using FindObjectOfType<Spawner>().SpawnNext(); OBS only works if the gameobject is called Spawner
    public void SpawnNext()
    {
        int i = Random.Range(0, groups.Length);
        
        if(groups[i] == null)
        {
            SpawnNext();
            return;
        }
        
        if(playerInput != null)
        {
            instanceGroup = groups[i];

            PlayerController instanceController = Instantiate(groups[i], playerSpawnpoint, Quaternion.identity).AddComponent<PlayerController>();

            instanceController.input = playerInput;
            instanceController.thrusterPower = thrustPower;

        }
        else
        {
            Debug.Log("NO PLAYER INPUT SELECTED IN SPAWNER");
        }
    }
}
