using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Vector3 playerSpawnpoint;
    public PlayerInput playerInput;

    public float thrustPower = 750;

    public GameObject[] groups;
    GameObject instanceGroup;

    private GameObject playerParent;


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
    

    public void SpawnNext()
    {

            int i = Random.Range(0, groups.Length);

            if (groups[i] == null)
            {
                SpawnNext();
                return;
            }

            if (playerInput != null)
            {
                instanceGroup = groups[i];

                PlayerController instanceController = Instantiate(groups[i], playerSpawnpoint, Quaternion.identity).AddComponent<PlayerController>();

                instanceController.input = playerInput;
                instanceController.thrusterPower = thrustPower;
                instanceController.gameObject.transform.parent = playerParent.transform;

            }
            else
            {
                Debug.Log("NO PLAYER INPUT SELECTED IN SPAWNER");
            }
        
    }
    public void RemoveRemainingBlocks()
    {
        int limit = transform.parent.childCount;

        for (int i = 0; i < limit; i++)
        {
            if(transform.parent.GetChild(i) != null &&
               transform.parent.GetChild(i).GetComponent<PlayerController>() != null)
            {
                Destroy(transform.parent.GetChild(i).gameObject);
            }
        }
    }

    public GameObject PlayerParent { set { playerParent = value; } }
}
