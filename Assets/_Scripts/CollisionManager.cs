using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollisionManager : MonoBehaviour
{
    public CubeBehaviour[] cubeActors;
    //public 
    public SphereProperties[] sphereActors;

    // Start is called before the first frame update
    void Start()
    {
        cubeActors = FindObjectsOfType<CubeBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        // check for cubes
        for (int i = 0; i < cubeActors.Length; i++)
        {
            for (int j = 0; j < cubeActors.Length; j++)
            {
                if (i != j)
                {
                    CheckAABBs(cubeActors[i], cubeActors[j]);
                }
            }
        }

        // collect whatever number of sphere you have and get ready to use it
        sphereActors = FindObjectsOfType<SphereProperties>();

        for (int i = 0; i < sphereActors.Length; i++)
        {
          //  Debug.Log("Length = " + sphereActors.Length);
            for (int j = 0; j < cubeActors.Length; j++)
            {
                SphereAABB(sphereActors[i], cubeActors[j]);
            }
            for (int k = 0; k < sphereActors.Length; k++)
            {
                if (i != k)
                {
                    SphereSphereCollision(sphereActors[i], sphereActors[k]);
                }
            }
        }
    }
        

    public static void CheckAABBs(CubeBehaviour a, CubeBehaviour b)
    {
        PhysicsBody aPB = a.GetComponent<PhysicsBody>();
        if ((a.min.x <= b.max.x && a.max.x >= b.min.x) &&
            (a.min.y <= b.max.y && a.max.y >= b.min.y) &&
            (a.min.z <= b.max.z && a.max.z >= b.min.z))
        {
            //Debug.Log("Collision happening on " + a);
            if (!a.contacts.Contains(b))
            {
                //Debug.Break();
               // Debug.Log("In contains function..");
                a.contacts.Add(b);
                a.isColliding = true;
                b.isColliding = true;
                // if the acting object is a box
                if (a.tag == "Box") 
                {
                    a.GetComponent<PhysicsBody>().CollisionResponseCubeCube(b);   
                }
            }
        }
        else
        {
            if (a.contacts.Contains(b))
            {
                //Debug.Log("In remove contains function..");
                a.contacts.Remove(b);
                a.isColliding = false;
                b.isColliding = false;
                if (a.tag == "Box" && aPB.hitFloor == false)
                {
                    aPB.acceleration.y = aPB.gravity;
                }
            }

        }

       
    }

    // need to check collision for spheres
    public static void SphereAABB(SphereProperties sphere, CubeBehaviour cube)
    {
        // Clamping, getting the closest point of the collision
        float x = Math.Max(cube.min.x, Math.Min(sphere.transform.position.x, cube.max.x));
        float y = Math.Max(cube.min.y, Math.Min(sphere.transform.position.y, cube.max.y));
        float z = Math.Max(cube.min.z, Math.Min(sphere.transform.position.z, cube.max.z));

        // Storing this point
        Vector3 clampingPoint = new Vector3(x,y,z);
        // for the actual vector in between
        Vector3 distancePoint = sphere.transform.position - clampingPoint; 
        

        // Calculating Distance now
        double distance = Math.Sqrt(
            (x - sphere.transform.position.x) * (x - sphere.transform.position.x) +
            (y - sphere.transform.position.y) * (y - sphere.transform.position.y) +
            (z - sphere.transform.position.z) * (z - sphere.transform.position.z)
        );

        //Vector3 distanceVector = cube.transform.position - sphere.transform.position; // distance from object 1 to object 2
        //Vector3 normalizedVector = Vector3.Normalize(distanceVector); // normalized distance vector

        // Means sphere is colliding with cube
        if (distance < sphere.getRadius())
        {
            float depth = sphere.getRadius() - (float)distance;

            if (!cube.sphereContacts.Contains(sphere))
            {
                //Vector3 closestPoint = new Vector3(x, y, z);
                Vector3 normalVector = distancePoint.normalized;
                //Vector3 reversedVector = ((clampingPoint * (-1))).normalized;
                if (cube.tag == "Box")
                {
                    Debug.Log("Normal Vector: " + normalVector);
                }
                //Debug.Log("reversed normal Vector: " + reversedVector);
                //Debug.Break();

                //Debug.Log(sphere.name + " is Colliding with " + cube.name + " !");
                sphere.isColliding = true; // this checks for a split second
                cube.sphereContacts.Add(sphere);
                sphere.GetComponent<PhysicsBody>().CollisionResolveSphereCube(cube, normalVector);
                cube.isColliding = true;
            }
        }
        else
        {
            if (cube.sphereContacts.Contains(sphere))
            {
                //Debug.Log(sphere.name + " is Not Colliding with " + cube.name + " !");
                cube.sphereContacts.Remove(sphere);
                sphere.isColliding = false;
               // cube.isColliding = false;
            }
        }
    }

    public static void SphereSphereCollision(SphereProperties a, SphereProperties b)
    {
        double distance = Math.Sqrt((a.transform.position.x - b.transform.position.x) * (a.transform.position.x - b.transform.position.x)
                                 + (a.transform.position.y - b.transform.position.y) * (a.transform.position.y - b.transform.position.y)
                                 + (a.transform.position.z - b.transform.position.z) * (a.transform.position.z - b.transform.position.z));

            float sumRadius = a.getRadius() + b.getRadius();

        if (distance <= sumRadius)
        {
            Debug.Log("Sphere Collision");
        }
    }


}
