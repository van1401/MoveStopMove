using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public class PlayerController : CharacterController
{
    [SerializeField] float speed;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Rigidbody rb;
    public Transform skin;
    public BulletController bulletPrefab;

    void Start()
    {
        
    }

    // Update is called once per frame
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

    public void Shoot()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
