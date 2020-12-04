using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBall();
        }
    }

    public void ShootBall()
    {
        GameObject bullet = BulletPool.sharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            bullet.transform.position = transform.position;
            bullet.GetComponent<SphereProperties>().forward = transform.forward;
            bullet.SetActive(true);
        }
    }
}
