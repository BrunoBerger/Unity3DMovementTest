using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  // yes
  public Camera playerCam;
  public Animator animatior;
  public CharacterController controller;
  public LayerMask GroundLayer;

  // describe player locomotion
  public float gravity;
  public float moveSpeed;
  public float jumpHeight;
  public float turnSmoothTime = 0.1f;
  private float turnSmoothVelocity;
  public float speed;
  private bool playerIsGrounded;
  private Vector3 lastPos;
  private Vector3 rawDirection;
  private Vector3 moveDir;
  public Vector3 playerExtraVelocity;
  
  // debugging
  private Color gizCol;


  void Start()
  {
    lastPos = transform.position;
  }
  
  // Update is called once per frame
  void Update()
  {
    // Ground-collision-check
    //get the radius of the players capsule collider, and make it a tiny bit smaller than that
    float radius = controller.radius * 0.9f;
    //get the position (assuming its right at the bottom) and move it up by almost the whole radius
    Vector3 pos = transform.position + Vector3.up*(radius*0.5f);
    //returns true if the sphere touches something on that layer
    bool playerIsGrounded = Physics.CheckSphere(pos, radius, GroundLayer);

    // constant forces on the character
    lastPos = transform.position;
    playerExtraVelocity.y += gravity * Time.deltaTime;
    if (playerIsGrounded)
    {
      playerExtraVelocity.y = 0;
      gizCol = Color.red;
    }
    else
    {
      gizCol = Color.yellow;
    }

    // Horizontal Movement
    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");
    rawDirection = new Vector3(h, 0, v).normalized;
    if (rawDirection.magnitude >= 0.1f)
    {
      float targetAngle = Mathf.Atan2(rawDirection.x, rawDirection.z) * Mathf.Rad2Deg + playerCam.transform.eulerAngles.y;
      float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
      transform.rotation = Quaternion.Euler(0f, angle, 0f);

      moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
      controller.Move(moveDir * moveSpeed * Time.deltaTime);
    }


    // Jumping
    if (Input.GetButtonDown("Jump") && playerIsGrounded)
    {
      playerExtraVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    speed = getPlayerVelcity().magnitude;

    // Actually move the player and update their animation
    controller.Move(playerExtraVelocity * Time.deltaTime);
    animatior.SetFloat("speed", speed);
    animatior.SetBool("isGrounded", playerIsGrounded);
    
    // Debug-Stuff
    Debug.DrawRay(transform.position+ new Vector3(0,1,0), transform.forward*2, Color.blue);
    Debug.DrawRay(transform.position+ new Vector3(0,1,0), moveDir*2, Color.green);
    Debug.DrawRay(transform.position+ new Vector3(0,1,0), playerExtraVelocity, Color.white);
  }
  void OnDrawGizmos()
  {
    // visualisation of Ground Collider
    float radius = controller.radius * 0.9f;
    Vector3 pos = transform.position + Vector3.up*(radius*0.5f);
    Gizmos.color = gizCol;
    Gizmos.DrawWireSphere(pos, radius);
  }

  // getters for info on player
  public bool getPlayerGrounded()
  {
    return playerIsGrounded;
  }
  public Vector3 getPlayerVelcity()
  {
    Vector3 pastDir = transform.position - lastPos;
    Debug.DrawRay(transform.position+ new Vector3(0,1,0), pastDir*20, Color.red);
    return pastDir;
  }

}