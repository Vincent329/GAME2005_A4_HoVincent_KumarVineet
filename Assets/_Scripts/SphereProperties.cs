using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereProperties : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 position;
    float lifeDuration = 500; // milliseconds I guess?
    float lifeStart;

    void OnAwake()
    {
        lifeStart = 0;        
    }

    // Update is called once per frame
    void Update()
    {
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
