using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public Camera playerCam;
  public Animator animatior;
  public CharacterController controller;
  public LayerMask GroundLayer;


  public float gravity;
  public float moveSpeed;
  public float jumpHeight;
  public float turnSmoothTime = 0.1f;
  public float turnSmoothVelocity;

  private bool playerIsGrounded;

  public Vector3 rawDirection;
  private Vector3 moveDir;
  private Vector3 playerExtraVelocity;


  void Start()
  {

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
    Debug.Log(playerIsGrounded);

    // Add all relevant constant forces on the character
    playerExtraVelocity.y += gravity * Time.deltaTime;
    if (playerIsGrounded)
      playerExtraVelocity.y = 0;


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
    // Jumping
    if (Input.GetButtonDown("Jump") && playerIsGrounded)
    {
      playerExtraVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
      // Debug.Log("Jump Input!");
    }
    // Apply move to player
    controller.Move(playerExtraVelocity * Time.deltaTime);




    // Update animation
    animatior.SetFloat("speed", controller.velocity.magnitude);
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
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(pos, radius);
  }

  // Getters
  public bool getPlayerGrounded()
  {
    return playerIsGrounded;
  }

}