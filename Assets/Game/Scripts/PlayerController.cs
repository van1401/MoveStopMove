using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public class PlayerController : CharacterController
{
    [SerializeField] float speed;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform skin;
    [SerializeField] Transform transhoot;

    

    private void Start()
    {
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
        StartCoroutine(Shoot());
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

    IEnumerator Shoot()
    {
        if (currentAmmo == maxAmmoSize)
        {
            Instantiate(bulletPrefab, transhoot.transform.position, transform.rotation);
            currentAmmo -= 1;
            yield return new WaitForSeconds(2f);
            if (currentAmmo < maxAmmoSize)
            {
                currentAmmo += 1;
            }
        }
    }
}
