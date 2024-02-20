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
    public float turnSpeed, lifeTime;

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

    IEnumerator BulletMove(Transform bulletTransform, Vector3 direction)
    {
        for (int i = 0; i < 60 * lifeTime; i++)
        {
            bulletTransform.Translate(direction * Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }
        SmartPool.Instance.Despawn(gameObject);
    }
}
