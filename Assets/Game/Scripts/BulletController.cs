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
    private float velocity = 5;
    public float turnSpeed;
    
    

    void Update()
    {
        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Enemy");
            Destroy(gameObject);
        }
        if (!stopProjectile) 
        {
            if(Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                stopProjectile = true;
                Destroy(gameObject);
            }    
        }
        Move();
    }

    void Move()
    {
          transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), velocity * Time.deltaTime);
          transform.Rotate(0, 10, 0 * turnSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.gameObject.CompareTag("Enemy"))
        {
           Destroy(this.gameObject);
        }
    }
}
