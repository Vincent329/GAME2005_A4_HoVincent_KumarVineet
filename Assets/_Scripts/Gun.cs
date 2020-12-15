using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool inPauseState = false;
    public static Gun sharedInstance;
    // Start is called before the first frame update
    void Start()
    {
        sharedInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // GUN UPDATE FOR GETTING INPUT OF LMB!
        if (Input.GetMouseButtonDown(0) && !inPauseState)
        {
           
            ShootBall();
        }
    }

    // get the component of the bullet
    public void ShootBall()
    {
        GameObject bullet = BulletPool.sharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            //bullet.GetComponent<SphereProperties>().forward = transform.forward;
            bullet.GetComponent<PhysicsBody>().forward = transform.forward;
            //Debug.Log("Bullet Forward " + bullet.GetComponent<PhysicsBody>().forward);
            bullet.SetActive(true);
            
            //Debug.Log("Velocity on Spawn " + bullet.GetComponent<PhysicsBody>().velocity);
        }
    }
}
