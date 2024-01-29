using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : Character
{
    public static BotController Instance;
    public float wanderRadius, wanderTimer;
    public NavMeshAgent agent;
    private float timer;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }


    void Update()
    {
        if (!isShooting)
        {
            Move();
        }
    }



    void Move()
    {
        timer += Time.deltaTime;
        if (timer >= wanderTimer)
        {   
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
        if (agent.velocity != Vector3.zero)
        {
            ChangeAnim("Run");
        }
        else {ChangeAnim("IsIdle");}
    }    


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.gameObject.CompareTag("Bullet"))
        {
            hp -= 1;
            if (hp <= 0 )
            {
                SmartPool.Instance.Despawn(this.gameObject);
                this.PostEvent(EventID.EnemyKill);
            }
        }    
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}

