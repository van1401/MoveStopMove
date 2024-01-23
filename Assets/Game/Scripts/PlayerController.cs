using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class PlayerController : Character
{
    public float dist;
    public float checkRange;
    public Transform nearestEnemy;
    public LayerMask enemyLayer;
    public float detectionRange;
    public static PlayerController Instance;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }    
    }


    void Update()
    {
        Move();
        CheckForEnemies();

    }


    void Move()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 nextPoint = JoystickController.direct * speed * Time.deltaTime + transform.position;
            transform.position = CheckGround(nextPoint);
            if (JoystickController.direct != Vector3.zero)
            {
                skin.forward = JoystickController.direct;
                ChangeAnim("Run");
            }
        }
        else
        {
            ChangeAnim("IsIdle");
        }
    }   


    
    void CheckForEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);

        foreach (Collider enemyCollider in hitColliders)
        {
            Transform  enemyTransform = enemyCollider.transform;
            //Debug.Log("Detected enemy at position: " + enemyTransform.position);

            nearestEnemy = FindNearestEnemy();
            dist = Vector3.Distance(transform.position, nearestEnemy.transform.position);
            if (nearestEnemy != null && dist < checkRange)
            {
                StartCoroutine(Shoot(nearestEnemy));
                skin.LookAt(nearestEnemy);  
                transhoot.LookAt(nearestEnemy);
            }
            else
            {
                return;
            }
        }
    }
    Transform FindNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);

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

    public Vector3 CheckGround(Vector3 nextPoint)
    {
        RaycastHit hit;

        if (Physics.Raycast(nextPoint, Vector3.down, out hit, 2f, groundLayer))
        {
            return hit.point + Vector3.up * 0.01f;
        }
        return transform.position;
    }
}
