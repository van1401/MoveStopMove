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
        Move();
        CheckDistance();
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
        else if(isShooting)
        { ChangeAnim("Attack"); }
        else if (!isDead)
        { ChangeAnim("Idle"); }
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
            Throw();
            ChangeAnim("Attack");
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
