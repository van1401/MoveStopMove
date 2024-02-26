using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Sirenix.OdinInspector;
using static UnityEngine.GraphicsBuffer;
using Core.Pool;
using DG.Tweening;
using System;

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
    public List<Character> targets = new List<Character>();
    protected Character target;
    public bool isDead = false;
    public bool isShooting = false;
    public bool isRunning = false;
    public bool isDancing = false;


    public void Start()
    {
        OnInit();
    }


    private void OnInit()
    {
        hp = 1;
        currentAmmo = 1; 
    }

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
        if (currentAmmo > 0 && targetedEnemyObj != Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            targetedEnemyObj = nearestEnemy.transform.position;
            bulletPrefab.GetComponent<BulletController>().target = targetedEnemyObj;
            bulletPrefab.GetComponent<BulletController>().targetSet = true;
            transform.LookAt(targetedEnemyObj);
            var clone = SmartPool.Instance.Spawn(bulletPrefab, transhoot.transform.position, transform.rotation);
            clone.gameObject.GetComponent<BulletController>().target = targetedEnemyObj;
            clone.gameObject.GetComponent<BulletController>().targetSet = true;
            currentAmmo -= 1;
            yield return new WaitForSeconds(1f);
            if (currentAmmo < 1)
            { currentAmmo += 1; }
            isShooting = false;
        }
    }




    protected void OnDead()
    {
        isDead = true; 
        rb.velocity = Vector3.zero;
        ChangeAnim("Dead");
        SmartPool.Instance.Despawn(gameObject);
    }


    protected void scaleUp()
    {
        Vector3 orginalScale = model.transform.localScale;
        Vector3 scaleSize = orginalScale + (orginalScale * 0.05f);
        model.transform.DOScale(scaleSize, 1);
    }


}

