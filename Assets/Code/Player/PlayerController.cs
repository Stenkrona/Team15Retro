using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidBody;
    private Transform playerTransform;
    public float thrusterPower = 100;
    public float rotateSpeed = 100;
    public PlayerInput input;


    private void Start()
    {
        if (gameObject.GetComponent<Rigidbody>())
            playerRigidBody = gameObject.GetComponent<Rigidbody>();
        if (gameObject.GetComponent<Transform>())
            playerTransform = gameObject.GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        //Up and down thursters
        if (Input.GetKey(input.upThruster))
            playerRigidBody.AddRelativeForce(0, thrusterPower * Time.deltaTime, 0, ForceMode.Acceleration);
        if (Input.GetKey(input.downThruster))
            playerRigidBody.AddRelativeForce(0, -thrusterPower * Time.deltaTime, 0, ForceMode.Acceleration);
        //Left and right thrusters
        if (Input.GetKey(input.rightThurster))
            playerRigidBody.AddRelativeForce(0, 0, -thrusterPower * Time.deltaTime, ForceMode.Acceleration);
        if (Input.GetKey(input.leftThurster))
            playerRigidBody.AddRelativeForce(0, 0, thrusterPower * Time.deltaTime, ForceMode.Acceleration);
        //Rotation
        float xAxisValue = Input.GetAxis(input.horizontalAxis);
        playerTransform.Rotate(new Vector3(xAxisValue * rotateSpeed * Time.deltaTime, 0, 0));
    }
}
