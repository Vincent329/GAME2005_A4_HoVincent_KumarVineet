using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Math.Max requirement
using System;

[System.Serializable]
public class SphereProperties : MonoBehaviour
{
    // Start is called before the first frame update
    //public Vector3 forward;
    public float speed = 5;
    float lifeDuration = 700; // counting up to 500 frames
    float lifeStart;
    public bool isColliding;
    static int staticNumber;
    int bulletNumber;
   // public PhysicsBody pb;

    // Sphere experimental(ly driving crazy now.)
    private MeshFilter meshFilter;
    private float m_radius;
    private Bounds m_bounds;
    Vector3 m_scale;
    Vector3 min,max;

   // public PhysicsBody
    void Awake()
    {
        lifeStart = 0;
        bulletNumber = staticNumber++;

        // Calculate Radius
        meshFilter = GetComponent<MeshFilter>();
        m_bounds = meshFilter.mesh.bounds;

        // Sphere is same all side, so any one of x,y,z is ok.
        m_radius = m_bounds.extents.x;
        m_radius = Math.Max(m_bounds.extents.x, Math.Max(m_bounds.extents.y, m_bounds.extents.z));
        max = Vector3.Scale(m_bounds.max, transform.localScale) + transform.position;
        min = Vector3.Scale(m_bounds.min, transform.localScale) + transform.position;
        // Confirmed
        //Debug.Log("Radius = " + m_radius);
        // pb = GetComponent<PhysicsBody>();
    }

    // Update is called once per frame
    void Update()
    {
        //forward = transform.forward;
        //transform.Translate(forward * speed * Time.deltaTime); 
        lifeStart++;
        if (lifeStart > lifeDuration)
        {
           // Despawn();
        }
        /*switch(bulletNumber)
        {
            case 0:
            Debug.Log("Bullet number = " + bulletNumber );
            break;

            case 1:
             Debug.Log("Bullet number = " + bulletNumber);
            break;

            default:
            break;
        }*/
    }

    void Despawn()
    {
        gameObject.SetActive(false);
        lifeStart = 0;
    }

  
    // Got Skeletal Mesh Right, now use it for physics
     void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,m_radius);
    }

     public float getRadius()
     {
         return m_radius;
     }

}