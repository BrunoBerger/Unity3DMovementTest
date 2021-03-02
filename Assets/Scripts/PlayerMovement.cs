using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public Camera playerCam;
  public Animator animatior;
  public CharacterController controller;

  public float gravity;
  public float moveSpeed;
  public float jumpHeight;
  public float turnSmoothTime = 0.1f;
  public float turnSmoothVelocity;
  
  public bool playerIsGrounded;

  private Vector3 rawDirection;
  private Vector3 moveDir;
  private Vector3 playerExtraVelocity;


  void Start()
  {

  }
  
  // Update is called once per frame
  void Update()
  {
    // Horizontal Movement
    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");
    rawDirection = new Vector3(h, 0, v).normalized;

    if (rawDirection.magnitude >= 0.1f && playerIsGrounded==true)
    {
      float targetAngle = Mathf.Atan2(rawDirection.x, rawDirection.z) * Mathf.Rad2Deg + playerCam.transform.eulerAngles.y;
      float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
      transform.rotation = Quaternion.Euler(0f, angle, 0f);

      moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
      controller.Move(moveDir * moveSpeed * Time.deltaTime);
    }
  

    // // Jumping
    if (Input.GetButtonDown("Jump") && playerIsGrounded)
      playerExtraVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);


    // Add gravity
    playerExtraVelocity.y += gravity * Time.deltaTime;
    controller.Move(playerExtraVelocity * Time.deltaTime);



    // Update animation
    animatior.SetFloat("speed", controller.velocity.magnitude);
    animatior.SetBool("isGrounded", playerIsGrounded);
    
    Debug.DrawRay(transform.position+ new Vector3(0,1,0), transform.forward*5, Color.blue);
    Debug.DrawRay(transform.position+ new Vector3(0,1,0), moveDir*5, Color.green);
  }


  void OnCollisionStay(Collision collisionInfo)
  {
    if (collisionInfo.gameObject.tag == "Terrain")
    {
      playerIsGrounded = true;
    }
  }
  void OnCollisionExit(Collision collisionInfo)
  {
    if (collisionInfo.gameObject.tag == "Terrain")
    {
      playerIsGrounded = false;
    }
  }

}