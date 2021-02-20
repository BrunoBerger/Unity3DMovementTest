using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float force = 20f;
    public float torque;
    public float maxSpeed = 20f;

    public Animator animatior;
    public Rigidbody playerRB;
    public Vector3 playerVelocity;
    private float potGain;
    public bool isGrounded;

    public Vector3 inputDrive;
    public float inputTurn;


    void Start()
    {
      torque = 3f;
      playerRB = this.GetComponent<Rigidbody>();

    }
    // Update is called once per frame
    void Update()
    {
      inputDrive = new Vector3(0f,0f,Input.GetAxis("Vertical"));
      inputTurn = Input.GetAxis("Horizontal");
      potGain = playerRB.velocity.magnitude + inputDrive.magnitude;

      // Update animation
      playerVelocity = playerRB.velocity;
      animatior.SetFloat("speed", playerVelocity.magnitude);
      animatior.SetBool("isGrounded", isGrounded);
      
    }
    void FixedUpdate()
    {
      if (isGrounded==true)
      {
        playerRB.AddRelativeTorque(0,torque*inputTurn,0,ForceMode.Impulse);
        if (potGain < maxSpeed)
        {
          moveChar(inputDrive);
        }
      }
    }


    void moveChar(Vector3 direction)
    {
      playerRB.AddRelativeForce(direction * force);
    }



    void OnCollisionEnter(Collision collisionInfo)
    {
      if (collisionInfo.gameObject.name == "Terrain")
      {
        isGrounded = true;
      }
    }
    void OnCollisionExit(Collision collisionInfo)
    {
      if (collisionInfo.gameObject.name == "Terrain")
      {
        isGrounded = false;
      }
    }
}