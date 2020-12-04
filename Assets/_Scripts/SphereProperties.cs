using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereProperties : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 forward;
    public float speed = 10;
    float lifeDuration = 500; // counting up to 500 frames
    float lifeStart;

    void Awake()
    {
        lifeStart = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //forward = transform.forward;
        transform.Translate(forward * speed * Time.deltaTime); 
        lifeStart++;
        if (lifeStart > lifeDuration)
        {
            Despawn();
        }
    }

    void Despawn()
    {
        gameObject.SetActive(false);
        lifeStart = 0;
    }

}