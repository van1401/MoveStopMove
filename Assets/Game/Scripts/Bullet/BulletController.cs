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
        Rotate();        
    }

    void Rotate() 
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, target.z), velocity * Time.deltaTime);
        transform.Rotate(0, 10, 0 * turnSpeed * Time.deltaTime);      
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DetectRange"))
        {
            SmartPool.Instance.Despawn(gameObject);
        }
    }

    //public void OnInit(Character character, Transform target)
    //{
    //    this.character = character;
    //    this.target = target;
    //    transform.forward = (target.position - transform.position).normalized;  
    //    SmartPool.Instance.Despawn(gameObject);
    //}

    
}
