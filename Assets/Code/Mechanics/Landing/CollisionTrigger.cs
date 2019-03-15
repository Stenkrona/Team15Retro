using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
       // EventManager.TriggerEvent("OnCollisionLander");

        if(collision.gameObject.tag == "block")
        {
            GameObject gb = collision.gameObject;

            Quaternion temp = gb.transform.rotation;




            Debug.Log(temp.eulerAngles);

        }

    }

}
