﻿using System.Collections;
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
    public GameObject bulletPrefab, model, detectionRange, weapon;
    public float speed, checkRange, checkingRadius = 1000, searchInterval = 3f;
    protected float dist, lastSearchTime = Mathf.NegativeInfinity;
    public Rigidbody rb;
    public Transform skin, transhoot, nearestEnemy;
    private string currentAnim;
    public int currentAmmo = 1, hp = 1;
    public Vector3 targetEnemy;
    public LayerMask groundLayer, enemyLayer;
    public bool canAttack = true, canMove = true, isDead = false;
  

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

    protected virtual IEnumerator Shoot(Vector3 targetedEnemyObj)
    {
        if (targetedEnemyObj == Vector3.zero)
        {
            yield return null;
        }
        if (currentAmmo > 0 && targetedEnemyObj != Vector3.zero && canAttack)
        {
            canMove = false;
            rb.velocity = Vector3.zero;
            targetedEnemyObj = nearestEnemy.transform.position;
            bulletPrefab.GetComponent<BulletController>().target = targetedEnemyObj;
            bulletPrefab.GetComponent<BulletController>().targetSet = true;
            transform.LookAt(targetedEnemyObj);
            var clone = SmartPool.Instance.Spawn(bulletPrefab, transhoot.transform.position, transform.rotation);
            clone.gameObject.GetComponent<BulletController>().target = targetedEnemyObj;
            clone.gameObject.GetComponent<BulletController>().targetSet = true;
            canMove = true;
            currentAmmo -= 1;
            yield return new WaitForSeconds(1.5f);
            if (currentAmmo < 1)
            {
                currentAmmo += 1;
            }
        }
    }


    public void Throw()
    {
        weapon.SetActive(false);
        StartCoroutine(Shoot(nearestEnemy.transform.position));
    }



    protected void scaleUp()
    {
        Vector3 orginalScale = model.transform.localScale;
        Vector3 scaleSize = orginalScale + (orginalScale * 0.05f);
        model.transform.DOScale(scaleSize, 1);
    }
    public void ResetAttack()
    {
        weapon.SetActive(true);
        print("check anim");
        ChangeAnim("Idle");
        canAttack = true;
    }
}

