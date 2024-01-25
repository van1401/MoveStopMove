using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : Character
{
    public float wanderRadius;
    public float wanderTimer;

    private NavMeshAgent agent;
    private float timer;
  

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }


    void Update()
    {
        timer += Time.deltaTime;
        Move();
    }



    void Move()
    {
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            ChangeAnim("Run");
            timer = 0;
        }
        else
        {
            ChangeAnim("IsIdle");
        }
    }    


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.gameObject.CompareTag("Bullet"))
        {
            hp -= 1;
            if (hp <= 0 )
            {
                this.PostEvent(EventID.EnemyKill);
                Destroy(this.gameObject); 
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
