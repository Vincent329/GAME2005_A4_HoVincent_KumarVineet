using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[System.Serializable]
public class PhysicsBody : MonoBehaviour
{
    // Start is called before the first frame update
    public float mass;
    public float friction; // use for slowing down, implement soon

    // static and kinetic friction
    public float staticFriction;
    public float kineticFriction;

    public Vector3 velocity;
    public Vector3 acceleration;
    public float restitution; // bounciness
    public bool hitFloor; // if the box contacts the floor

    public float speed; // speed of velocity
    public float gravity;
    public Vector3 forward; // forward direction vector (sphere use)
    
    public int typeOfObject;

    // Debug Text
    Text debugText;

    void OnEnable()
    {
        if (gameObject.GetComponent<SphereProperties>() != null) // if the physics body is a sphere
        {
            velocity = forward * speed;
            acceleration = new Vector3(0.0f, gravity, 0.0f);
           // friction = 0.5f;
        }
        else if(gameObject.GetComponent<CubeBehaviour>() != null) // if the physics body is a cube
        {
            acceleration = new Vector3(0.0f, gravity, 0.0f);
            hitFloor = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        velocity += (acceleration * Time.deltaTime); // initially this is going to fall down by gravity
       
        //Debug.Log("Position = " + transform.position);
        if (gameObject.GetComponent<CubeBehaviour>() != null) // cube property
        {
            if (gameObject.GetComponent<CubeBehaviour>().isColliding)
            {
                velocity.y = 0;
                acceleration.y = 0;
                velocity *= (1 - friction/10);

                if (Vector3.Magnitude(velocity) < Vector3.Magnitude(acceleration) && Vector3.Magnitude(velocity) > 0.0f)
                {
                    velocity = new Vector3(0.0f, 0.0f, 0.0f);
                    acceleration = new Vector3(0.0f, 0.0f, 0.0f);
                }
            }
        }

        transform.position += velocity * Time.deltaTime;
    }

    // creating a separate class for cube cube collision response for now
    public void CollisionResponseCubeCube(CubeBehaviour cube)
    {
        //transform.position -= velocity * Time.fixedDeltaTime; // reposition
        //if (cube.tag == "Box" && hitFloor)
        //{
        //    cube.GetComponent<PhysicsBody>().velocity.x += velocity.x;
        //    cube.GetComponent<PhysicsBody>().velocity.z += velocity.z;
        //    velocity.z *= -1 * restitution;  
        //    velocity.x *= -1 * restitution;
            
        //    //Debug.Log("Velocity " + velocity);
        //}
        if (cube.tag == "Floor")
        {
            velocity.y *= 0.0f;
            acceleration.y *= 0.0f;
            //Debug.Log("Velocity " + velocity);
            hitFloor = true;
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
    }

    // responding to the collision by changing the relative velocity of objects
    public void CollisionResponseCube(CubeBehaviour cube)
    {
        //PhysicsBody pb = GetComponent<PhysicsBody>();
        PhysicsBody cubePB = cube.GetComponent<PhysicsBody>();

        Vector3 finalVelocity;
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
       // Debug.Log("Cube movement = " + velocity);
        if (cube.tag == "Floor")
        {
            velocity.y *= -1f * restitution;
            velocity.x *= restitution;
            velocity.z *= restitution;
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
            
            //Debug.Log("Velocity of sphere = "+ (Vector3.Magnitude(velocity)));
            // Earlier threshold 0.15 (bug)
            if (Vector3.Magnitude(velocity) < 0.61f)
            {
                this.GetComponent<SphereProperties>().Despawn();
            }
        }
        else if (cube.tag == "WallZ")
        {
            // check which direction of z and x axis and then perform the rebound
            velocity.z *= -1 * restitution;
            Debug.Log("In Debug Log");
          
        }
        else if (cube.tag == "WallX")
        {
            velocity.x *= -1 * restitution;
        }
        else if (cube.tag == "Box")
        {

            Vector3 relativeVelocity = cubePB.velocity - velocity;

            // find if the objects are moving towards each other
            float velAlongNormal = Vector3.Dot(relativeVelocity, normal);

            // Find tangent for friction
            Vector3 tangentVectorForFriction = relativeVelocity - velAlongNormal * normal;
            // normalization
            //tangentVectorForFriction.Normalize();
            //Debug.Log("Velocity Along Normal " + velAlongNormal);

            float e = Mathf.Min(restitution, cubePB.restitution);
            //float minFriction = Mathf.Min((float)friction, (float)cubePB.friction);
//             Debug.Log("MinFriction: " + minFriction);

            // Directly
            float j = -(1 - e) * velAlongNormal;

//            // With Friction
//            float jt = -(1 - e) * Vector3.Dot(relativeVelocity, tangentVectorForFriction);
////          Debug.Log("Dot Product: " + Vector3.Dot(relativeVelocity, tangentVectorForFriction));
            
//            friction = Mathf.Sqrt(friction * cubePB.friction);
           
            // Debug.Log("j with restitution change " + j);
            float inverseMassSphere = 1 / mass;
            float inverseMassCube = 1 / cubePB.mass;

            // Normal
            j /= (inverseMassSphere + inverseMassCube);

            // With Friction
            //jt /= (inverseMassCube + inverseMassSphere);
            //Debug.Log("j value " + j);

            //jt = Mathf.Min(jt, j * friction);

            Vector3 impulse = j * normal;
            Debug.Log("Initial Impulse " + impulse);
            float impulseMag = Vector3.Magnitude(impulse);
            Debug.Log("Initial Impulse Magnitude " + impulseMag);

            // impulse is acceleration
            
            velocity -= inverseMassSphere * impulse * 3;
            velocity *= restitution;

            if (frictionCheck(cube, impulseMag, cubePB.friction))
            {
                return;
            }
            
            cubePB.velocity.x += inverseMassCube * impulse.x * 3;
            cubePB.velocity.z += inverseMassCube * impulse.z * 3;


  

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

        transform.position -= velocity * Time.deltaTime;
        sphere.transform.position -= spheresPB.velocity * Time.deltaTime;
        //Vector3 finalVelocity;
        //finalVelocity =
        //      ((mass - spheresPB.mass) / (mass + spheresPB.mass)) * velocity
        //      + ((2 * spheresPB.mass) / (mass + spheresPB.mass)) * spheresPB.velocity;

        //velocity -= finalVelocity * restitution / 30;
        //spheresPB.velocity += finalVelocity * restitution / 30;
        Vector3 distance = transform.position - sphere.transform.position;
        Vector3 normal = Vector3.Normalize(distance);

        Vector3 relativeVelocity = spheresPB.velocity - velocity;

        // find if the objects are moving towards each other
        float velAlongNormal = Vector3.Dot(relativeVelocity, normal);
        float e = Mathf.Min(restitution, spheresPB.restitution);
        float j = -(1 - e) * velAlongNormal;

        float inverseMassSphere = 1 / mass;
        j /= (inverseMassSphere + inverseMassSphere);


        Vector3 impulse = j * normal;
       

        // impulse is acceleration

        velocity -= inverseMassSphere * impulse * 3;
        spheresPB.velocity += inverseMassSphere * impulse * 3;

    }

    public bool frictionCheck(CubeBehaviour cube, float impulse, float staticFriction)
    {
        PhysicsBody cubePB = cube.GetComponent<PhysicsBody>();

        // F = m*a
        float impulseForce = mass * impulse; // applied force of the sphere
        Debug.Log("Sphere Mass" + mass);
        Debug.Log("Impulse " + impulseForce);
        // FrictionForce = mk * m * g
        float frictionForce = staticFriction * cubePB.mass * gravity;
        Debug.Log("Static Friction " + Math.Abs(frictionForce));

        if (Math.Abs(impulseForce) >= Math.Abs(frictionForce))
        {
            Debug.Log("Impulse Should Happen");
            return false;
        }
        return true;
    }
}
