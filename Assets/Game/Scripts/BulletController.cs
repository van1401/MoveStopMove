using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int speed;
    public Transform  target;

    void Start()
    {
        if (target == null)
        {
            //target = GameObject.Find("Bot").GetComponent<Transform>();
            target = GameObject.FindWithTag("Enemy").transform;
        }
        if (target != null)
        {
            transform.position = target.position;
        }
    }
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += Vector3.MoveTowards(transform.position, target.transform.position, 10f * speed * Time.deltaTime);
        //dung movetowards
        transform.Rotate(0, 10, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
