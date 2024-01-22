using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public class PlayerController : Character
{
    public float dist;
    public float checkRange;
    public Transform Target;
    public LayerMask enemyLayer;
    public float detectionRange;


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
            Transform enemyTransform = enemyCollider.transform;
            //Debug.Log("Detected enemy at position: " + enemyTransform.position);

            Target = enemyTransform;
            dist = Vector3.Distance(transform.position, Target.transform.position);
            if (dist <= checkRange)
            {
                StartCoroutine(Shoot(Target.gameObject));

                skin.LookAt(Target);  
                transhoot.LookAt(Target);
            }
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
}
