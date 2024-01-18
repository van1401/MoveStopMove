using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletController : MonoBehaviour
{
    public int speed;
    public GameObject target;
    public bool targetSet;
    public bool stopProjectile;
    public float velocity = 5;
    public float turnSpeed;
    public float damage;


    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
        }
        Move();
        if (!stopProjectile ) 
        {
            if(Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                stopProjectile = true;
                Destroy(gameObject);
            }    
        
        }
    }
    
    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, velocity * Time.deltaTime);
        transform.Rotate(0, 10, 0 * turnSpeed * Time.deltaTime);
        //transform.position += new Vector3(0, 0, 1) * speed * Time.deltaTime;
        //transform.Rotate(0, 10, 0 * turnSpeed * Time.deltaTime);  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
