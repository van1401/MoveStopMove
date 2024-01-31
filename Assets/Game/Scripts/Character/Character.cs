using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Sirenix.OdinInspector;
using static UnityEngine.GraphicsBuffer;
using Core.Pool;
using DG.Tweening;

public class Character : MonoBehaviour
{
    public Animator anim;
    public GameObject bulletPrefab, model, detectionRange;
    public float speed, checkRange, checkingRadius = 1000, searchInterval = 3f;
    protected float dist, lastSearchTime = Mathf.NegativeInfinity;
    public Rigidbody rb;
    public Transform skin, transhoot, nearestEnemy;
    private string currentAnim;
    public int currentAmmo = 1, hp = 1;
    public LayerMask groundLayer, enemyLayer;
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

    protected IEnumerator Shoot(Vector3 targetedEnemyObj)
    {
        if(targetedEnemyObj == Vector3.zero)
        {
            yield return null;
        }

        else if (currentAmmo > 0 && targetedEnemyObj != Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            targetedEnemyObj = nearestEnemy.transform.position;
            bulletPrefab.GetComponent<BulletController>().target = targetedEnemyObj;
            bulletPrefab.GetComponent<BulletController>().targetSet = true;
            var clone = SmartPool.Instance.Spawn(bulletPrefab, transhoot.transform.position, transform.rotation);
            clone.gameObject.GetComponent<BulletController>().target = targetedEnemyObj;
            clone.gameObject.GetComponent<BulletController>().targetSet = true;
            currentAmmo -= 1;
            yield return new WaitForSeconds(1.5f);
            if (currentAmmo < 1)
            {
                currentAmmo += 1;
            }
        }
    }
    protected void scaleUp()
    {
        Vector3 orginalScale = model.transform.localScale;
        Vector3 scaleSize = orginalScale + (orginalScale * 0.05f);
        model.transform.DOScale(scaleSize, 1);
    }

    protected virtual void CheckDistance()
    {
        nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null)
        {
            return;
        }
        dist = Vector3.Distance(transform.position, nearestEnemy.transform.position);
        if (nearestEnemy != null && dist < checkRange)
        {
            isShooting = true;
            ChangeAnim("IsAttack");
            StartCoroutine(Shoot(nearestEnemy.transform.position));
            skin.LookAt(nearestEnemy.transform.position);
            transhoot.LookAt(nearestEnemy.transform.position);
        }
        else
        {
            isShooting = false;
        }
    }

    protected virtual Transform FindNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkingRadius, enemyLayer);

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

