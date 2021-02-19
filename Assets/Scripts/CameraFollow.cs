  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // target is assigned in the editor
    public GameObject target;
    private Vector3 targetPos;

    // cam-behaviour
    public float turnSpeed = 4.0f;
    public Vector3 camAngle;

    // Start is called before the first frame update
    void Start()
    {
      camAngle.Set(0, 2, -4);
    }

    // Update is called once per frame
    void Update()
    {
      targetPos = target.transform.position;
      camAngle = Quaternion.AngleAxis (Input.GetAxis("Mouse X") * turnSpeed,
                                        Vector3.up) * camAngle;
      transform.position = targetPos + camAngle;
      transform.LookAt(targetPos);
    }

}