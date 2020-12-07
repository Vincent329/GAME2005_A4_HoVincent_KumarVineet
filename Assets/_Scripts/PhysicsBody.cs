﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicsBody : MonoBehaviour
{
    // Start is called before the first frame update
    public float mass;
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
    void Update()
    {
        velocity += acceleration * Time.deltaTime;

        //Debug.Log("Velocity = " + velocity);
        //velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        //Debug.Log(acceleration);
        
    }
}
