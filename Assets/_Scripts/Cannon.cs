using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public static BulletPool bulletPoolRef;

    // Start is called before the first frame update
    void Start()
    {
        //bulletPoolRef = BulletPool.GetBulletPool();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Fire()
    {
        //bulletPoolRef.FireBullet(transform.position);
    }
}
