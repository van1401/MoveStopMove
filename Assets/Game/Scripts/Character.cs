using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Sirenix.OdinInspector;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    public Animator anim;
    public GameObject bulletPrefab;
    public int currentAmmo, maxAmmoSize = 1;
    public int hp = 1;
    public float speed;
    public Rigidbody rb;
    public Transform skin;
    public Transform transhoot;
    public Transform nearestEnemy;
    public Vector3 lastEnemyPosition;
    private string currentAnim;
    protected float dist;
    public float checkRange;
    public LayerMask groundLayer, enemyLayer;
    public float detectionRange;
    protected bool isShooting = false;
    
    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            if (currentAnim != null)
            {
                anim.ResetTrigger(currentAnim);
            }
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    protected IEnumerator Shoot(Transform targetedEnemyObj)
    {
        if (currentAmmo == maxAmmoSize)
        {
            Instantiate(bulletPrefab, transhoot.transform.position,Quaternion.identity); 
            bulletPrefab.GetComponent<BulletController>().target =  targetedEnemyObj;
            bulletPrefab.GetComponent<BulletController>().targetSet = true;
            currentAmmo -= 1;
            yield return new WaitForSeconds(1f);
            if (currentAmmo < maxAmmoSize)
            {
                currentAmmo += 1;
            }
        }
    }
    protected void CheckDistance()
    {
        nearestEnemy = FindNearestEnemy();
        dist = Vector3.Distance(transform.position, nearestEnemy.transform.position);
        if (nearestEnemy != null && dist < checkRange)
        {
            isShooting = true;
            ChangeAnim("IsAttack");
            StartCoroutine(Shoot(nearestEnemy));
            skin.LookAt(nearestEnemy);
            transhoot.LookAt(nearestEnemy);
        }
        else
        {
            isShooting = false;
            return;
        }
    }
    Transform FindNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);

        Transform nearestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider collider in colliders)
        {
            Vector3 directionToEnemy = collider.transform.position - currentPosition;
            float sqrDistanceToEnemy = directionToEnemy.sqrMagnitude;

            if (sqrDistanceToEnemy < closestDistanceSqr)
            {
                closestDistanceSqr = sqrDistanceToEnemy;
                nearestEnemy = collider.transform;
            }
        }
        return nearestEnemy;
    }
}
