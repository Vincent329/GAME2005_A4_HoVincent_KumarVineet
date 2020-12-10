using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PhysicsBody : MonoBehaviour
{
    // Start is called before the first frame update
    public float mass;
    public double friction; // use for slowing down, implement soon
    public Vector3 velocity;
    public Vector3 acceleration;
    public float restitution; // bounciness
    public bool hitFloor;

    public float speed; // speed of velocity
    public float gravity;
    public Vector3 forward;

    public int typeOfObject;

    // Debug Text
    Text debugText;

    void OnEnable()
    {
        if (gameObject.GetComponent<SphereProperties>() != null)
        {
            velocity = forward * speed;
            acceleration = new Vector3(0.0f, gravity, 0.0f);
            friction = 0.5;
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
        velocity += (acceleration * Time.deltaTime);
        transform.position += velocity * Time.deltaTime;
        if (typeOfObject == 1)
        {
            //Debug.Log("Velocity = " + velocity);
        }
        //Debug.Log("Position = " + transform.position);
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
        //Debug.Log("In Response of Cube Cube");
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
            if (velocity.x <= 0.15)
            {
                //Debug.Break();
                //Debug.Log("Landed and less than 1.0f");
                velocity.x = 0.0f;
                
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
        Debug.Log("Cube movement = " + velocity);
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
            Debug.Log("In Debug Log");
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
            //Debug.Log("BoxCollision");
            Vector3 relativeVelocity = cubePB.velocity - velocity;
            //Debug.Log("Relative Velocity " + relativeVelocity);

            // find if the objects are moving towards each other
            float velAlongNormal = Vector3.Dot(relativeVelocity, normal);
            
            // Find tangent for friction
            Vector3 tangentVectorForFriction = relativeVelocity - Vector3.Dot(relativeVelocity, normal) * normal;
            // normalization
            //tangentVectorForFriction.Normalize();
            //Debug.Log("Velocity Along Normal " + velAlongNormal);

            //if (velAlongNormal > 0)
            //{
            //    Debug.Log("this happening?");
            //    return;
            //}

            float e = Mathf.Min(restitution, cubePB.restitution);
            float minFriction = Mathf.Min((float)friction, (float)cubePB.friction);
            //Debug.Log("Restitution: " + e);


            // Directly
            float j = -(1 - e) * velAlongNormal;

            // With Friction
            float jt = -(1 - e) * Vector3.Dot(relativeVelocity, tangentVectorForFriction);

            Vector3 frictionImpulse;

            friction = Math.Sqrt(friction * cubePB.friction);
           
            // Debug.Log("j with restitution change " + j);
            float inverseMassSphere = 1 / mass;
            float inverseMassCube = 1 / cubePB.mass;
            //Debug.Log("Inverse of Sphere " + inverseMassSphere);
            //Debug.Log("Inverse of Cube " + inverseMassCube);
        
            // Normal
            j /= (inverseMassSphere + inverseMassCube);

            // With Friction
            jt /= (inverseMassCube + inverseMassSphere);
            //Debug.Log("j value " + j);

            Vector3 impulse = jt * normal;
           // Debug.Log("Impulse: " + impulse);

          
           
            velocity -= inverseMassSphere * impulse;
            //velocity *= restitution;
            cubePB.velocity.x += inverseMassCube * impulse.x;
            cubePB.velocity.z += inverseMassCube * impulse.z;


            // COMMENTING -----
            //velocity -= inverseMassSphere * frictionImpulse;
            //velocity *= restitution;
            //cubePB.velocity.x += inverseMassCube * frictionImpulse.x;
            //cubePB.velocity.z += inverseMassCube * frictionImpulse.z;



            // Applying friction

            //jt /= (inverseMassSphere + inverseMassCube);

            //friction = Math.Sqrt(friction * cubePB.friction);
            //float maxFriction = Mathf.Max(jt, (-1)*j * (float)friction);
            //float minFriction = Mathf.Min(jt, j * (float)friction);
            //cubePB.velocity.x += inverseMassCube * minFriction;
            //cubePB.velocity.z += inverseMassCube * minFriction;

            //Debug.Log("Friction = " + Random.Range(minFriction, maxFriction));


            //tangentVectorForFriction = relativeVelocity - ((relativeVelocity * ));

            //Debug.Log("NewVelocity " + velocity);
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
