using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyStats))]
public class EnemyMovement : MonoBehaviour
{
    EnemyStats eStats;
    NavMeshAgent agent;
    Rigidbody rb;
    Transform player;
    Vector3 velocity;
    private void Awake()
    {   agent = GetComponent<NavMeshAgent>();
        eStats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody>();
        agent.speed = eStats.mStats.moveSpeed;
        agent.acceleration = agent.speed;
        player = GameObject.Find("PlayerBody").transform;
        //agent.updatePosition = false;
        velocity = Vector3.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SetPos", 0, .1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move();
    }
    void SetPos()
    {
        agent.SetDestination(player.position);
        
    }
    void Move()
    {
        
        SetPos();
        //transform.position = Vector3.Lerp(transform.position, agent.nextPosition, eStats.mStats.moveSpeed);


    }
    
}
