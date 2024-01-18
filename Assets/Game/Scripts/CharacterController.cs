using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterController : MonoBehaviour
{
    public Animator anim;
    private string currentAnim;
    public GameObject bulletPrefab;
    public int currentAmmo, maxAmmoSize = 1;
    public int hp = 1;
    public float speed;
    public LayerMask groundLayer;
    public Rigidbody rb;
    public Transform skin;
    public Transform transhoot;

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

    protected IEnumerator Shoot(GameObject targetedEnemyObj)
    {
        if (currentAmmo == maxAmmoSize)
        {
            Instantiate(bulletPrefab, transhoot.transform.position,transform.rotation);
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
}
