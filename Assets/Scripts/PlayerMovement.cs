using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float force = 20f;
    public float torque;
    public float maxSpeed = 20f;

    public Rigidbody rb;
    public Vector3 velocity;
    private float potGain;
    public bool isgrounded;

    public Vector3 inputDrive;
    public float inputTurn;


    void Start()
    {
      torque = 3f;
      rb = this.GetComponent<Rigidbody>();

    }
    // Update is called once per frame
    void Update()
    {
      inputDrive = new Vector3(0f,0f,Input.GetAxis("Vertical"));
      inputTurn = Input.GetAxis("Horizontal");
      potGain = rb.velocity.magnitude + inputDrive.magnitude;
    }
    void FixedUpdate()
    {
      if (isgrounded==true)
      {
        rb.AddRelativeTorque(0,torque*inputTurn,0,ForceMode.Impulse);
        if (potGain < maxSpeed)
        {
          moveChar(inputDrive);
        }
      }
    }


    void moveChar(Vector3 direction)
    {
      rb.AddRelativeForce(direction * force);
    }



    void OnCollisionEnter(Collision collisionInfo)
    {
      if (collisionInfo.gameObject.name == "Terrain")
      {
        isgrounded = true;
      }
    }
    void OnCollisionExit(Collision collisionInfo)
    {
      if (collisionInfo.gameObject.name == "Terrain")
      {
        isgrounded = false;
      }
    }
}