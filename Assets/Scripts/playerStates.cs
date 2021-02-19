using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStates : MonoBehaviour
{
    public Animator animatior;
    public Rigidbody playerRB;

    public Vector3 playerVelocity;
    public bool playerOnGround;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerVelocity= playerRB.velocity;
        animatior.SetFloat("speed", playerVelocity.magnitude);

        Debug.DrawRay(transform.position, playerVelocity*1000, Color.red);
    }
}
