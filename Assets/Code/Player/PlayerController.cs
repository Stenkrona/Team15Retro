using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidBody;
    private Transform playerTransform;
    public float thrusterPower = 1000;
    public float rotateSpeed = 100;
    public PlayerInput input;


    private void Start()
    {
        //get the transform and the rigidbody from the gameobject and set them
        if (gameObject.GetComponent<Rigidbody>())
            playerRigidBody = gameObject.GetComponent<Rigidbody>();
        if (gameObject.GetComponent<Transform>())
            playerTransform = gameObject.GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        if (gameObject.GetComponent<Rigidbody>())
        {
            //Up and down thursters
            if (Input.GetKey(input.upThruster))
                playerRigidBody.AddRelativeForce(0, thrusterPower * Time.deltaTime, 0, ForceMode.Acceleration);
            if (Input.GetKey(input.downThruster))
                playerRigidBody.AddRelativeForce(0, -thrusterPower * Time.deltaTime, 0, ForceMode.Acceleration);
            //Left and right thrusters
            if (Input.GetKey(input.rightThurster))
                playerRigidBody.AddRelativeForce(-thrusterPower * Time.deltaTime, 0, 0, ForceMode.Acceleration);
            if (Input.GetKey(input.leftThurster))
                playerRigidBody.AddRelativeForce(thrusterPower * Time.deltaTime, 0, 0, ForceMode.Acceleration);
        }
        if (gameObject.GetComponent<Transform>())
        {
            //Rotation
            float xAxisValue = Input.GetAxis(input.horizontalAxis);
            playerTransform.Rotate(new Vector3(0, 0, -xAxisValue * rotateSpeed * Time.deltaTime));
        }
    }
}
