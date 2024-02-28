using Core.Pool;
using System.Collections;
using System.Collections.Generic;
using UniRx.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int speed, damage = 1;
    public Vector3 target;
    public bool targetSet;
    private float velocity = 5;
    public float turnSpeed;
    public GameObject shooter;

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

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }

    private void OnTriggerEnter(Collider other)
    {
        BotController enemy = other.GetComponent<BotController>();
        PlayerController player = other.GetComponent<PlayerController>();
        if (enemy != null && shooter != null && shooter.CompareTag("Player"))
        {
            this.PostEvent(EventID.EnemyKill);
            enemy.TakeDamage(damage);

        }
        else if (player != null && shooter != null && shooter.CompareTag("Enemy"))
        {
            player.TakeDamage(damage);
        }
    }
}

    //public void OnInit(Character character, Transform target)
    //{
    //    this.character = character;
    //    this.target = target;
    //    transform.forward = (target.position - transform.position).normalized;  
    //    SmartPool.Instance.Despawn(gameObject);
    //}



