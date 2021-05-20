using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public Transform goal;
    NavMeshAgent agent;
       
    // Start is called before the first frame update
    void Start () 
    {
        agent = GetComponent<NavMeshAgent>();
        goal = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = goal.position;
    }
}
