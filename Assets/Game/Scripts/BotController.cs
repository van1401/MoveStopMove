using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : Character
{

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.gameObject.CompareTag("Bullet"))
        {
            hp -= 1;
            if (hp <= 0 )
            {
                Destroy(this.gameObject);
            }
        }    
    }
}
