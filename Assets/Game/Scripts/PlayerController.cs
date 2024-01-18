using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public class PlayerController : CharacterController
{
    public Transform Target;
    public float dist;
    public float checkRange;

    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Enemy").transform;    
    }



    void Update()
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

        dist =  Vector3. Distance(transform.position, Target.position);
        if(dist < checkRange) 
        {
            StartCoroutine(Shoot(Target.gameObject));
            skin.LookAt(Target.position);
            transhoot.LookAt(Target.position);
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
