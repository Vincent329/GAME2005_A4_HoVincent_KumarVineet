﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contacts
{
    public GameObject contact;
    public Vector3 getNormal;
}

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
    void FixedUpdate()
    {
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
        sphereActors = FindObjectsOfType<SphereProperties>();

        for (int i = 0; i < sphereActors.Length; i++)
        {
            Debug.Log("Length = " + sphereActors.Length);
            for (int j = 0; j < cubeActors.Length; j++)
            {
                SphereAABB(sphereActors[i], cubeActors[j]);
            }
        }
    }

    public static void CheckAABBs(CubeBehaviour a, CubeBehaviour b)
    {
        if ((a.min.x <= b.max.x && a.max.x >= b.min.x) &&
            (a.min.y <= b.max.y && a.max.y >= b.min.y) &&
            (a.min.z <= b.max.z && a.max.z >= b.min.z))
        {
           
            if (!a.contacts.Contains(b))
            {
                a.contacts.Add(b);
                a.isColliding = true;
                b.isColliding = true;
                Vector3 normals = a.transform.position - b.transform.position;
            }
        }
        else
        {
            if (a.contacts.Contains(b))
            {
                a.contacts.Remove(b);
                a.isColliding = false;
                b.isColliding = false;

            }

        }
    }

    // need to check collision for spheres
    public static void SphereAABB(SphereProperties sphere, CubeBehaviour cube)
    {
        // Clamping
        float x = Math.Max(cube.min.x, Math.Min(sphere.transform.position.x, cube.max.x));
        float y = Math.Max(cube.min.y, Math.Min(sphere.transform.position.y, cube.max.y));
        float z = Math.Max(cube.min.z, Math.Min(sphere.transform.position.z, cube.max.z));

        // Calculating Distance now
        double distance = Math.Sqrt(
            (x - sphere.transform.position.x) * (x - sphere.transform.position.x) +
            (y - sphere.transform.position.y) * (y - sphere.transform.position.y) +
            (z - sphere.transform.position.z) * (z - sphere.transform.position.z)
        );

        Vector3 distanceVector = cube.transform.position - sphere.transform.position; // distance from object 1 to object 2
        Vector3 normalizedVector = Vector3.Normalize(distanceVector); // normalized distance vector



        if (distance < sphere.getRadius())
        {
            float depth = sphere.getRadius() - (float)distance;

            if (!cube.sphereContacts.Contains(sphere))
            {
                //Debug.Log(sphere.name + " is Colliding with " + cube.name + " !");
                sphere.isColliding = true; // it's only changing when it's the floor
                cube.isColliding = true;
                cube.sphereContacts.Add(sphere);
                sphere.GetComponent<PhysicsBody>().CollisionResponseCube(cube);
            }
        }
        else
        {
            if (cube.sphereContacts.Contains(sphere))
            {
                //Debug.Log(sphere.name + " is Not Colliding with " + cube.name + " !");
                cube.sphereContacts.Remove(sphere);
                sphere.isColliding = false;
            }
        }
    }
    public static void SphereSphereCollision(SphereProperties a, SphereProperties b)
    {

    }

   
}
