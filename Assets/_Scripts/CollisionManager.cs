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
    public CubeBehaviour[] actors;

    // Start is called before the first frame update
    void Start()
    {
        actors = FindObjectsOfType<CubeBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < actors.Length; i++)
        {
            for (int j = 0; j < actors.Length; j++)
            {
                if (i != j)
                {
                    CheckAABBs(actors[i], actors[j]);
                }
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
                Vector3 normals = a.transform.position - b.transform.position;
            }
        }
        else
        {
            if (a.contacts.Contains(b))
            {
                a.contacts.Remove(b);
                a.isColliding = false;
            }
           
        }
    }

    // need to check collision for spheres
    public static void SphereAABBCheck()
    {

    }
}
