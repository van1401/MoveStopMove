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
    public LayerMask playerLayer;
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
        CheckDistance();
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

    protected override Transform FindNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);

        Transform nearestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider collider in colliders)
        {
            Vector3 directionToEnemy = collider.transform.position - currentPosition;
            float sqrDistanceToEnemy = directionToEnemy.sqrMagnitude;

            if (sqrDistanceToEnemy < closestDistanceSqr)
            {
                closestDistanceSqr = sqrDistanceToEnemy;
                nearestEnemy = collider.transform;
            }
        }
        return nearestEnemy;
    }

    protected override void CheckDistance()
    {
        nearestEnemy = FindNearestEnemy();
        if (Time.time - lastSearchTime >= searchInterval)
        {
            nearestEnemy = FindNearestEnemy();
            lastSearchTime = Time.time;
        }
        if (nearestEnemy == null)
        {
            return;
        }
        dist = Vector3.Distance(transform.position, nearestEnemy.transform.position);
        if (nearestEnemy != null && dist < checkRange)
        {
            isShooting = true;
            //StartCoroutine(Shoot(lastEnemyPosition));
            //ChangeAnim("IsAttack");
            //skin.LookAt(lastEnemyPosition);
            //transhoot.LookAt(lastEnemyPosition);
        }
        else
        {
            isShooting = false;
        }
    }

}

