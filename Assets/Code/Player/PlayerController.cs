using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    private Transform playerTransform;
    public float thrusterPower = 200;
    public float rotateSpeed = 100;
    public PlayerInput input;
    public ParticleSystem upThrusterParticle;
    public ParticleSystem downThrusterParticle;
    public ParticleSystem leftThrusterParticle;
    public ParticleSystem rightThrusterParticle;
    [HideInInspector] public float velocity;

    private void Start()
    {
        //get the transform and the rigidbody from the gameobject and set them
        if (!gameObject.GetComponent<Rigidbody2D>())
            gameObject.AddComponent<Rigidbody2D>();

        playerRigidBody = gameObject.GetComponent<Rigidbody2D>();
        playerRigidBody.gravityScale = 0.2f;
        if (gameObject.GetComponent<Transform>())
            playerTransform = gameObject.GetComponent<Transform>();
        GetThrusterParticleSystems();
        if (thrusterPower == 0)
            thrusterPower = 200;
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
        velocity = playerRigidBody.velocity.magnitude;
        Debug.Log(velocity);
        

    }
    private void Update()
    {
        if (Input.GetKeyDown(input.upThruster) && upThrusterParticle)
            upThrusterParticle.Play();
        if (Input.GetKeyUp(input.upThruster) && upThrusterParticle.isPlaying)
            upThrusterParticle.Stop();
        if (Input.GetKeyDown(input.downThruster) && downThrusterParticle)
            downThrusterParticle.Play();
        if (Input.GetKeyUp(input.downThruster) && downThrusterParticle.isPlaying)
            downThrusterParticle.Stop();
        if (Input.GetKeyDown(input.leftThurster) && leftThrusterParticle)
            leftThrusterParticle.Play();
        if (Input.GetKeyUp(input.leftThurster) && leftThrusterParticle.isPlaying)
            leftThrusterParticle.Stop();
        if (Input.GetKeyDown(input.rightThurster) && rightThrusterParticle)
            rightThrusterParticle.Play();
        if (Input.GetKeyUp(input.rightThurster) && rightThrusterParticle.isPlaying)
            rightThrusterParticle.Stop();

    }
    private void GetThrusterParticleSystems()
    {
        if(upThrusterParticle == null)
            upThrusterParticle = transform.GetChild(3).GetChild(0).GetComponent<ParticleSystem>();
        if(downThrusterParticle == null)
            downThrusterParticle = transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>();
        if(leftThrusterParticle == null)
            leftThrusterParticle = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
        if(rightThrusterParticle == null)
            rightThrusterParticle = transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
    }
}

