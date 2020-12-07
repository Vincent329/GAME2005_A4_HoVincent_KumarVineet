using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float groundLevel; // = GameObject.Find("Floor").transform.position.y;
    public Vector3 m_velocity, m_acceleration;
    private bool m_active;
    private float deltaTime = 1.0f / 60.0f;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("In Start Of Bullet");
        groundLevel = GameObject.Find("Floor").transform.position.y;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
        //if ()
        {
           //Move();
           // CheckCollision();
           //Debug.Log("Update -> Active()");
        }
    }

    void Move()
    {
        //m_velocity += m_acceleration;
        //transform.position += m_velocity * deltaTime;
    }

    void CheckCollision()
    {
        //Debug.Log(m_velocity.x );
        //Debug.Log("In Check Collision Of Bullets.");
        if(transform.position.y <= groundLevel)
        {
            m_velocity = new Vector3(0.0f, 0.0f, 0.0f);
            m_acceleration = new Vector3(0.0f, 0.0f, 0.0f);
            
        }
    }

    private void Reset()
    {
        transform.position = new Vector3(0.0f, groundLevel - 5.0f, 0.0f);
        m_velocity = new Vector3(0.0f, 0.0f, 0.0f);
        m_acceleration = new Vector3(0.0f, -0.98f, 0.0f);

        //m_active = false;
    }
    void Spawn()
    {

    }
}
