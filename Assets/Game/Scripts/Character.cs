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
    public GameObject bulletPrefab, model;
    public int currentAmmo = 1, maxAmmoSize = 1, hp = 1;
    public float speed, checkRange, detectionRange = 1000, searchInterval = 3f;
    protected float dist, lastSearchTime = Mathf.NegativeInfinity;
    public Rigidbody rb;
    public Transform skin, transhoot, nearestEnemy;
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
        if(targetedEnemyObj == Vector3.zero)
        {
            yield return null;
        }
        
        if (currentAmmo == maxAmmoSize)
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
            if (currentAmmo < maxAmmoSize)
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
        detectionRange += 100;
    }
}

