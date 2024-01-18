using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Animator anim;
    private string currentAnim;
    public BulletController bulletPrefab;
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

    protected IEnumerator Shoot()
    {
        if (currentAmmo == maxAmmoSize)
        {
            Instantiate(bulletPrefab, transhoot.transform.position, transform.rotation);
            currentAmmo -= 1;
            yield return new WaitForSeconds(2f);
            if (currentAmmo < maxAmmoSize)
            {
                currentAmmo += 1;
            }
        }
    }
}
