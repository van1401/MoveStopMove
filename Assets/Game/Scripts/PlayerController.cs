using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Character
{

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


        if (!isShooting)
        {
            Move();
        }
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
        else{ChangeAnim("IsIdle");}
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
