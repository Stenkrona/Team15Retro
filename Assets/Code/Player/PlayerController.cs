using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    private Transform playerTransform;
    public float thrusterPower = 1000;
    public float rotateSpeed = 100;
    public PlayerInput input;

    private void Start()
    {
        //get the transform and the rigidbody from the gameobject and set them
        if (!gameObject.GetComponent<Rigidbody2D>())
            gameObject.AddComponent<Rigidbody2D>();

        playerRigidBody = gameObject.GetComponent<Rigidbody2D>();
        playerRigidBody.gravityScale = 0.2f;
        if (gameObject.GetComponent<Transform>())
            playerTransform = gameObject.GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        if (gameObject.GetComponent<Rigidbody2D>())
        {
            //Up and down thursters
            if (Input.GetKey(input.upThruster))
                playerRigidBody.AddRelativeForce(new Vector2(0, thrusterPower * Time.deltaTime));
            if (Input.GetKey(input.downThruster))
                playerRigidBody.AddRelativeForce(new Vector2(0, -thrusterPower * Time.deltaTime));
            //Left and right thrusters
            if (Input.GetKey(input.rightThurster))
                playerRigidBody.AddRelativeForce(new Vector2(-thrusterPower * Time.deltaTime, 0));
            if (Input.GetKey(input.leftThurster))
                playerRigidBody.AddRelativeForce(new Vector2(thrusterPower * Time.deltaTime, 0));
        }
        if (gameObject.GetComponent<Transform>())
        {
            //Rotation
            float xAxisValue = Input.GetAxis(input.horizontalAxis);
            playerTransform.Rotate(new Vector3(0, 0, -xAxisValue * rotateSpeed * Time.deltaTime));
        }
    }
}
