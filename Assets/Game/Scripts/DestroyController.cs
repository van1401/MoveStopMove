using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyController : MonoBehaviour
{
    public int time;

    // Update is called once per frame
    void Update()
    {
        Destroy();
    }

    void Destroy()
    {
        time++;
        if (time == 500)
        {
            Destroy(this.gameObject);
            time = 0;
        }
    }
}
