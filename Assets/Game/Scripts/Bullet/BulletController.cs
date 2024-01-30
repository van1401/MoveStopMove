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
            SmartPool.Instance.Despawn(gameObject);
        }
        if (Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z)) < 0.3f)
        {
            SmartPool.Instance.Despawn(this.gameObject);
        }
        Move();        
    }

    void Move() 
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, target.z), velocity * Time.deltaTime);
        transform.Rotate(0, 10, 0 * turnSpeed * Time.deltaTime);      
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("RangedAttack"))
        {
            SmartPool.Instance.Despawn(this.gameObject);
        }
    }
}
