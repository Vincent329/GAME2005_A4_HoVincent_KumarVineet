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

    public float speed; // speed of velocity
    public float gravity;
    public Vector3 forward;

    // Debug Text
    Text debugText;

    void OnEnable()
    {
        velocity = forward * speed;
        acceleration = new Vector3(0.0f, gravity, 0.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      
        //Debug.Log(acceleration);
        if (gameObject.GetComponent<CubeBehaviour>() != null)
        {
            if(gameObject.GetComponent<CubeBehaviour>().isColliding)
            {
                // this foreach loop is handling cube on cubes, do another foreach check for the number of spheres
                foreach (CubeBehaviour cubes in gameObject.GetComponent<CubeBehaviour>().contacts)
                {
                    CollisionResponseCube(cubes);
                }
                //foreach (SphereProperties spheres in gameObject.GetComponent<CubeBehaviour>().sphereContacts)
                //{
                //    CollisionResponseSphere(spheres);
                //}
            }
        }
        velocity += acceleration * Time.fixedDeltaTime;
        transform.position += velocity * Time.fixedDeltaTime;
    }

    // responding to the collision by changing the relative velocity of objects
    public void CollisionResponseCube(CubeBehaviour cube)
    {
        //PhysicsBody pb = GetComponent<PhysicsBody>();
        PhysicsBody cubePB = cube.GetComponent<PhysicsBody>();

        Vector3 finalVelocity;
        transform.position -= velocity * Time.fixedDeltaTime; // reposition
        if (cube.tag == "Floor")
        {
            velocity.y *= -1f * restitution;
        }
        else if (cube.tag == "WallZ")
        {
            velocity.z *= -1 * restitution;
        }
        else if (cube.tag == "WallX")
        {
            velocity.x *= -1 * restitution;
        }
        else
        {
            finalVelocity = 
                ((mass - cubePB.mass) / (mass + cubePB.mass)) * velocity 
                + ((2 * cubePB.mass)/(mass + cubePB.mass)) * cubePB.velocity;
            velocity = finalVelocity * restitution;
        }
    }

    //public void CollisionResponseSphere(SphereProperties sphere)
    //{
    //    PhysicsBody spheresPB = sphere.GetComponent<PhysicsBody>();

    //    Vector3 finalVelocity;
    //    finalVelocity =
    //          ((mass - spheresPB.mass) / (mass + spheresPB.mass)) * velocity
    //          + ((2 * spheresPB.mass) / (mass + spheresPB.mass)) * spheresPB.velocity;
    //    velocity = finalVelocity * restitution;
    //}
}
