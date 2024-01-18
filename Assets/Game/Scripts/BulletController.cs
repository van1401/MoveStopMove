using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int speed;
    public Transform target;
    public float turnSpeed;

    void Update()
    {
        Move();
        transform.Rotate(0, 10, 0 * turnSpeed * Time.deltaTime);

    }
    
    void Move()
    {
        transform.position += new Vector3(0,0,1) * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
