using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMono<PoolManager>
{
    private Dictionary<int,Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();
    [SerializeField] private Pool[] pool = null;
    [SerializeField] private Transform objectPoolTransform = null;

    [System.Serializable]
    public struct Pool
    {
        public int poolSize;
        public GameObject prefab;
    }

    private void Start()
    {
        for( int i=0; i< pool.Length; i++)
        {
            CreatePool(pool[i].prefab, pool[i].poolSize);
        }
    }

    private void CreatePool(GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();
        string prefabName = prefab.name;

        GameObject parentGameObject = new GameObject(prefabName + "Anchor");

        parentGameObject.transform.SetParent(objectPoolTransform);

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());

            for (int i = 0; i <poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, parentGameObject.transform) as GameObject;
                newObject.SetActive(false);

                poolDictionary[poolKey].Enqueue(newObject);
            }
        }
    }

    public GameObject ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID() ;

        if(poolDictionary.ContainsKey(poolKey))
        {
            GameObject objecttToReuse = GetObjectFromPool(poolKey);

            ResetObject(position,rotation,objecttToReuse,prefab);

            return objecttToReuse;
        }
        else
        {
            Debug.Log("No object pool for" + prefab);
            return null;
        }

    }

    private void ResetObject(Vector3 position, Quaternion rotation, GameObject objecttToReuse, GameObject prefab)
    {
        objecttToReuse.transform.position = position;
        objecttToReuse.transform.rotation = rotation;

        objecttToReuse.transform.localScale = prefab.transform.localScale;
    }

    private GameObject GetObjectFromPool(int poolKey)
    {
        GameObject objectToResuse = poolDictionary[poolKey].Dequeue();
        poolDictionary[poolKey].Enqueue(objectToResuse);

        if(objectToResuse.activeSelf == true)
        {
            objectToResuse.SetActive(false);
        }

        return objectToResuse;
    }
}
