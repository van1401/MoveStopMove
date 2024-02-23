using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : Character
{

    public static PlayerController Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        this.RegisterListener(EventID.EnemyKill, (sender, param) =>
        {
            scaleUp();
        });
    }

    void Update()
    {
        ChangeLogicAnimation();
        CheckDistance();
    }


    void ChangeLogicAnimation()
    {
        if (isDead)
        {
            return;
        }

        if (isShooting)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        //Running
        if (Input.GetMouseButton(0) && JoystickController.direct != Vector3.zero)
        {
            Vector3 nextPoint = JoystickController.direct * speed * Time.deltaTime + transform.position;
            transform.position = CheckGround(nextPoint);
            skin.forward = JoystickController.direct;
            ChangeAnim("Run");
            isRunning = true;
        }
        //Shooting
        else if (nearestEnemy != null && dist < checkRange)
        {
            ChangeAnim("Attack");
            Throw();
        }
        else if(!isShooting && !isDead) 
        {
            ChangeAnim("Idle");
        }
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
    void CheckDistance()
    {
        nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null)
        {
            return;
        }
        dist = Vector3.Distance(transform.position, nearestEnemy.transform.position);
        if (nearestEnemy != null && dist < checkRange)
        {
            skin.LookAt(nearestEnemy.transform.position);
            transhoot.LookAt(nearestEnemy.transform.position);
        }
    }


    public Transform FindNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkingRadius, enemyLayer);

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
