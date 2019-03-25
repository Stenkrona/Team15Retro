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
    private Quaternion spawnRotation;


    // Start is called before the first frame update
    void Start()
    {
        if (groups.Length == 0)
        {
            Debug.Log("The Groups array has not been properly set up");
            return;
        }
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

                Vector3 temp = new Vector3(0, 0, Random.Range(0, 360));

                spawnRotation = Quaternion.Euler(temp);

                PlayerController instanceController = Instantiate(groups[i], playerSpawnpoint, spawnRotation ).AddComponent<PlayerController>();

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
