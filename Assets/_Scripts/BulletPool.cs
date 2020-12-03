using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool sharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    
    void Awake()
    {
        sharedInstance = this;    
    }
    void Start()
    {
        pooledObjects = new List<GameObject>(); // makes a new pool
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool); // instantiates all the bullets at the start
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public GameObject GetPooledObject()
    {
        //1
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            //2
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        //3   
        return null;
    }
}
