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
    public Character shooter;

    void Update()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
        }
        if (Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z)) < 0.3f)
        {
            Destroy(this.gameObject);
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
            Destroy(this.gameObject);
        }
    }

    public void SetShooter(Character shooter)
    {
        this.shooter = shooter;
    }

    private void OnTriggerEnter(Collider other)
    {
        BotController enemy = other.GetComponent<BotController>();
        PlayerController player = other.GetComponent<PlayerController>();
        if (enemy != null && shooter != null && ((shooter is PlayerController||shooter is BotController && shooter != enemy)))
        {
            this.PostEvent(EventID.EnemyKill);
            enemy.TakeDamage(damage);
            Debug.Log("enemy");

        }
        else if (player != null && shooter != null && shooter is BotController)
        {
            player.TakeDamage(damage);
            Debug.Log("player");
        }
    }
}



