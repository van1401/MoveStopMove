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
        Move(); 
        FindEnemyBot();
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
        else {ChangeAnim("Idle");}
    }    


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            hp -= 1;
            if (hp <= 0)
            {
                this.PostEvent(EventID.EnemyKill);
                Debug.Log("Check destroy");
                SmartPool.Instance.Despawn(gameObject);
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


    void FindEnemyBot()
    {
        LayerMask hitLayers = LayerMask.GetMask("Player") | LayerMask.GetMask("Enemy");
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkingRadius, hitLayers);
        List<Collider> filteredColliders = new List<Collider>(colliders);
        filteredColliders.RemoveAll(collider => collider.gameObject == gameObject);
        nearestEnemy = FindNearestEnemy(filteredColliders.ToArray());

        if(nearestEnemy == null)
        {
            return;
        }
        dist = Vector3.Distance(transform.position, nearestEnemy.transform.position);
        if (nearestEnemy != null && dist < checkRange)
        {
            //StartCoroutine(Shoot(nearestEnemy.transform.position));
            skin.LookAt(nearestEnemy.transform.position);
            transhoot.LookAt(nearestEnemy.transform.position);
        }
    }

    Transform FindNearestEnemy(Collider[] colliders)
    {
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
}



