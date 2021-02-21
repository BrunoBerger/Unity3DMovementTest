using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public Animator animatior;
  public Rigidbody playerRB;
  public Camera playerCam;


  public bool isGrounded;
  public float moveSpeed;


  public float turnSmoothTime = 0.1f;
  public float turnSmoothVelocity;
  public Vector3 moveDir;


  void Start()
  {
    playerRB = this.GetComponent<Rigidbody>();
  }
  
  // Update is called once per frame
  void Update()
  {
    
    Debug.DrawRay(transform.position+ new Vector3(0,1,0), transform.forward*5, Color.blue);
    Debug.DrawRay(transform.position+ new Vector3(0,1,0), moveDir*5, Color.green);

    // Jumping
    if (Input.GetButton("Jump"))
    {
      return;
    }

    // Update animation
    animatior.SetFloat("speed", playerRB.velocity.magnitude);
    animatior.SetBool("isGrounded", isGrounded);
    
  }

  void FixedUpdate()
  {
    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");
    Vector3 rawDirection = new Vector3(h, 0, v).normalized;
    
    if (rawDirection.magnitude >= 0.1f && isGrounded==true)
    {
      float targetAngle = Mathf.Atan2(rawDirection.x, rawDirection.z) * Mathf.Rad2Deg + playerCam.transform.eulerAngles.y;
      float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
      playerRB.rotation = Quaternion.Euler(0f, angle, 0f);

      moveDir = Quaternion.Euler(0f, targetAngle, 0f)* Vector3.forward;
      playerRB.velocity = (moveDir * moveSpeed * Time.deltaTime);
    }
  }


  void OnCollisionEnter(Collision collisionInfo)
  {
    if (collisionInfo.gameObject.tag == "Terrain")
    {
      isGrounded = true;
    }
  }
  void OnCollisionExit(Collision collisionInfo)
  {
    if (collisionInfo.gameObject.tag == "Terrain")
    {
      isGrounded = false;
    }
  }
}