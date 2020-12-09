using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicsBody : MonoBehaviour
{
    // Start is called before the first frame update
    public float mass;
    public float friction; // use for slowing down, implement soon
    public Vector3 velocity;
    public Vector3 acceleration;
    public float restitution; // bounciness
    public bool hitFloor;

    public float speed; // speed of velocity
    public float gravity;
    public Vector3 forward;

    // Debug Text
    Text debugText;

    void OnEnable()
    {
        if (gameObject.GetComponent<SphereProperties>() != null)
        {
            velocity = forward * speed;
            acceleration = new Vector3(0.0f, gravity, 0.0f);
        }
        else if(gameObject.GetComponent<CubeBehaviour>() != null)
        {
            acceleration = new Vector3(0.0f, gravity, 0.0f);
            hitFloor = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        if (gameObject.GetComponent<CubeBehaviour>() != null && hitFloor)
        {
            velocity.y = 0;
            acceleration.y = 0;
        }    

            //    if (gameObject.GetComponent<CubeBehaviour>().isColliding)
            //    {
            //        // this foreach loop is handling cube on cubes, do another foreach check for the number of spheres
            //        foreach (CubeBehaviour cubes in gameObject.GetComponent<CubeBehaviour>().contacts)
            //        {
            //            //Debug.Log(velocity);
            //            if (cubes.tag == "Box")
            //            {
            //                //Debug.Log(acceleration);
            //                CollisionResponseCubeCube(cubes);
            //            }
            //        }
            //        //foreach (SphereProperties spheres in gameObject.GetComponent<CubeBehaviour>().sphereContacts)
            //        //{
            //        //    CollisionResponseSphere(spheres);
            //        //}
            //    }
            //if (velocity.y != 0.0f)
            //{
            //Debug.Break();

            //}


        }

    // creating a separate class for cube cube collision response for now
    public void CollisionResponseCubeCube(CubeBehaviour cube)
    {
        Debug.Log("In Response of Cube Cube");
        if (cube.tag == "Floor")
        {
            velocity.y *= 0.0f;
            acceleration.y *= 0.0f;
            hitFloor = true;
        } 
        else if (cube.tag == "Box")
        {
            velocity.y *= 0.0f;
            acceleration.y *= 0.0f;
        }
    }


    // responding to the collision by changing the relative velocity of objects
    public void CollisionResponseCube(CubeBehaviour cube)
    {
        //PhysicsBody pb = GetComponent<PhysicsBody>();
        PhysicsBody cubePB = cube.GetComponent<PhysicsBody>();

        Vector3 finalVelocity;
        transform.position -= velocity * Time.deltaTime; // reposition
        if (cube.tag == "Floor")
        {
            velocity.y *= -1f * restitution;
            
            // Gave a min threshold value for velocity
            // so when it goes lower to that, v = 0, and ball will stop moving
            if (velocity.y <= 0.15)
            {
                //Debug.Break();
                //Debug.Log("Landed and less than 1.0f");
                velocity.y = 0.0f;
                
                // Experimenting with acceleration a bit. 
                acceleration.y = 0.0f;
            }
        }
        else if (cube.tag == "WallZ")
        {
            // check which direction of z and x axis and then perform the rebound
            velocity.z *= -1 * restitution;
            //if (velocity.z < 0)
            //{
            //    sphere.transform.position.z -= sphere.getRadius();
            //}
            
              
        }
        else if (cube.tag == "WallX")
        {
            velocity.x *= -1 * restitution;
        }
        else
        {
           // Debug.Break();
            finalVelocity =
                ((mass - cubePB.mass) / (mass + cubePB.mass)) * velocity
                + ((2 * cubePB.mass) / (mass + cubePB.mass)) * cubePB.velocity;
            velocity = finalVelocity * restitution;
        }
    }

    // using the impulse formula
    public void CollisionResolveSphereCube(CubeBehaviour cube, Vector3 normal)
    {
        //PhysicsBody pb = GetComponent<PhysicsBody>();
        PhysicsBody cubePB = cube.GetComponent<PhysicsBody>();
        transform.position -= velocity * Time.deltaTime; // reposition
        if (cube.tag == "Floor")
        {
            velocity.y *= -1f * restitution;

            // Gave a min threshold value for velocity
            // so when it goes lower to that, v = 0, and ball will stop moving
            if (velocity.y <= 0.15)
            {
                //Debug.Break();
                //Debug.Log("Landed and less than 1.0f");
                velocity.y = 0.0f;

                // Experimenting with acceleration a bit. 
                acceleration.y = 0.0f;
            }
        }
        else if (cube.tag == "WallZ")
        {
            // check which direction of z and x axis and then perform the rebound
            velocity.z *= -1 * restitution;
            //if (velocity.z < 0)
            //{
            //    sphere.transform.position.z -= sphere.getRadius();
            //}
        }
        else if (cube.tag == "WallX")
        {
            velocity.x *= -1 * restitution;
        }
        else if (cube.tag == "Box")
        {
            Debug.Log("BoxCollision");
            Vector3 relativeVelocity = cubePB.velocity - velocity;
            Debug.Log("Relative Velocity " + relativeVelocity);

            // find if the objects are moving towards each other
            float velAlongNormal = Vector3.Dot(relativeVelocity, normal);
            Debug.Log("Velocity Along Normal " + velAlongNormal);

            //if (velAlongNormal > 0)
            //{
            //    Debug.Log("this happening?");
            //    return;
            //}

            float e = Mathf.Min(restitution, cubePB.restitution);
            Debug.Log("Restitution: " + e);


            float j = -(1 - e) * velAlongNormal;
            Debug.Log("j with restitution change " + j);
            float inverseMassSphere = 1 / mass;
            float inverseMassCube = 1 / cubePB.mass;
            Debug.Log("Inverse of Sphere " + inverseMassSphere);
            Debug.Log("Inverse of Cube " + inverseMassCube);
        

            j /= (inverseMassSphere + inverseMassCube);
            Debug.Log("j value " + j);

            Vector3 impulse = j * normal;
            Debug.Log("Impulse: " + impulse);

            velocity -= inverseMassSphere * impulse * (speed/2);
            velocity *= restitution;
            cubePB.velocity.x += inverseMassCube * impulse.x;
            cubePB.velocity.z += inverseMassCube * impulse.z;
            

            Debug.Log("NewVelocity " + velocity);
            //Debug.Break();
        }
    }

    public void CollisionResponseSphere(SphereProperties sphere)
    {
        PhysicsBody spheresPB = sphere.GetComponent<PhysicsBody>();

        Vector3 finalVelocity;
        finalVelocity =
              ((mass - spheresPB.mass) / (mass + spheresPB.mass)) * velocity
              + ((2 * spheresPB.mass) / (mass + spheresPB.mass)) * spheresPB.velocity;
        velocity = finalVelocity * restitution;
    }
}
