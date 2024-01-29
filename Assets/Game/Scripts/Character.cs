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
    public GameObject bulletPrefab;
    public int currentAmmo = 1, maxAmmoSize = 1, hp = 1;
    public float speed, checkRange, detectionRange = 1000, searchInterval = 3f;
    protected float dist, lastSearchTime = Mathf.NegativeInfinity;
    public Rigidbody rb;
    public Transform skin, transhoot, nearestEnemy;
    public Vector3 lastEnemyPosition;
    private string currentAnim;
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
        if (currentAmmo == maxAmmoSize && rb.velocity == Vector3.zero)
        {
            SmartPool.Instance.Spawn(bulletPrefab, transhoot.transform.position, Quaternion.identity);
            Debug.Log("Shooting");
            bulletPrefab.GetComponent<BulletController>().target = targetedEnemyObj;
            bulletPrefab.GetComponent<BulletController>().targetSet = true;
            currentAmmo -= 1;
            yield return new WaitForSeconds(2f);
            if (currentAmmo < maxAmmoSize)
            {
                currentAmmo += 1;
            }
        }
    }

    protected virtual void CheckDistance()
    {
        nearestEnemy = FindNearestEnemy();
        if (Time.time - lastSearchTime >= searchInterval)
        {
            nearestEnemy = FindNearestEnemy();
            lastSearchTime = Time.time;
        }
        if (nearestEnemy == null)
        {
            return;
        }
        dist = Vector3.Distance(transform.position, nearestEnemy.transform.position);
        if (nearestEnemy != null && dist < checkRange)
        {
            lastEnemyPosition = nearestEnemy.transform.position;
            isShooting = true;
            StartCoroutine(Shoot(lastEnemyPosition));
            ChangeAnim("IsAttack");
            skin.LookAt(lastEnemyPosition);
            transhoot.LookAt(lastEnemyPosition);
        }
        else
        {
            isShooting = false;
        }
    }

    protected void scaleUp()
    {
        Vector3 orginalScale = transform.localScale;
        Vector3 scaleSize = orginalScale + (orginalScale * 0.05f);
        transform.DOScale(scaleSize, 1);
        checkRange += 0.1f;
    }

    protected virtual Transform FindNearestEnemy()
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

