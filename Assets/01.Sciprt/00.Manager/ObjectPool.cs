// # System
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

// # Unity
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ObjectPoolInfo
{
    public string objectName;
    public GameObject obj;

    public int poolLength => pool.Count;

    public Transform prarentObject;

    private Queue<GameObject> pool = new Queue<GameObject>();

    public void Enqueue(GameObject _object) => pool.Enqueue(_object);
    public GameObject Dequeue() => pool.Dequeue();
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public Dictionary<string, ObjectPoolInfo> poolDictionary = new Dictionary<string, ObjectPoolInfo>();

    public List<ObjectPoolInfo> poolList = new List<ObjectPoolInfo>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        PoolInit();
    }

    private void PoolInit()
    {
        foreach(ObjectPoolInfo pool in poolList)
        {
            poolDictionary.Add(pool.objectName, pool);
        }

        foreach(ObjectPoolInfo pool in poolDictionary.Values)
        {
            GameObject parent = new GameObject();

            pool.prarentObject = parent.transform;

            parent.transform.SetParent(transform);
            parent.name = pool.objectName;

            for(int i = 0; i < pool.poolLength; i++)
            {
                GameObject currentObject = Instantiate(pool.obj, parent.transform);
                currentObject.SetActive(false);

                pool.Enqueue(currentObject);
            }
        }
    }

    public GameObject SpawnFromPool(string name, Vector3 position)
    {
        ObjectPoolInfo currentPool = poolDictionary[name];

        if(currentPool.poolLength <= 0)
        {
            GameObject obj = Instantiate(currentPool.obj, currentPool.prarentObject);
            obj.SetActive(false);
            currentPool.Enqueue(obj);
        }

        GameObject currentObject = currentPool.Dequeue();
        currentObject.transform.position = position;
        currentObject.SetActive(true);
        return currentObject;
    }

    public void ReturnToPool(string name, GameObject currentObject)
    {
        ObjectPoolInfo pool = poolDictionary[name];

        currentObject.SetActive(false);
        currentObject.transform.SetParent(pool.prarentObject);

        pool.Enqueue(currentObject);
    }
}
