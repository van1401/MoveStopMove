using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int speed;
    public Vector3 target;
    public bool targetSet;
    private float velocity = 5;
    public float turnSpeed;

    void Update()
    {
        if (target == null)
        {
           Destroy(gameObject);
        }
        if (Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z)) < 0.3f)
        {
            Debug.Log("Checktransform");
            SmartPool.Instance.Despawn(gameObject);
        }
        Move();

    }

    void Move() 
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, target.z), velocity * Time.deltaTime);
        transform.Rotate(0, 10, 0 * turnSpeed * Time.deltaTime);      
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("RangedAttack"))
        {
            SmartPool.Instance.Despawn(gameObject);
        }
    }
}
