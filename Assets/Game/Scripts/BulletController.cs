using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int speed;
    public Transform target;
    public bool targetSet;
    private float velocity = 5;
    public float turnSpeed;

    void Update()
    {
        if (target == null)
        {
           Destroy(gameObject);
        }
        Move();
    }

    void Move()
    {
        target = PlayerController.Instance.nearestEnemy.transform;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), velocity * Time.deltaTime);
        transform.Rotate(0, 10, 0 * turnSpeed * Time.deltaTime);
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.gameObject.CompareTag("RangedAttack"))
        {
           Destroy(this.gameObject);
        }
    }
}
