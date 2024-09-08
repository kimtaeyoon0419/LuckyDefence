using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolInfo
{
    public string objectName;      // 풀의 이름
    public GameObject obj;         // 풀링할 기본 오브젝트 프리팹
    public int initialPoolSize = 10;  // 초기 생성할 오브젝트 수

    public int poolLength => pool.Count;  // 현재 큐에 있는 오브젝트 수

    public Transform parentObject;  // 풀의 부모 오브젝트 (Hierarchy 정리용)

    private Queue<GameObject> pool = new Queue<GameObject>();

    // 오브젝트 큐에 추가
    public void Enqueue(GameObject _object) => pool.Enqueue(_object);

    // 오브젝트 큐에서 제거
    public GameObject Dequeue() => pool.Dequeue();
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public Dictionary<string, ObjectPoolInfo> poolDictionary = new Dictionary<string, ObjectPoolInfo>();
    public List<ObjectPoolInfo> poolList = new List<ObjectPoolInfo>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PoolInit();
    }

    private void PoolInit()
    {
        foreach (ObjectPoolInfo pool in poolList)
        {
            poolDictionary.Add(pool.objectName, pool);
        }

        foreach (ObjectPoolInfo pool in poolDictionary.Values)
        {
            GameObject parent = new GameObject();
            pool.parentObject = parent.transform;
            parent.transform.SetParent(transform);
            parent.name = pool.objectName;

            for (int i = 0; i < pool.initialPoolSize; i++)
            {
                GameObject currentObject = Instantiate(pool.obj, parent.transform);
                currentObject.SetActive(false);
                pool.Enqueue(currentObject);
            }
        }
    }

    public GameObject SpawnFromPool(string name, Vector3 position)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogError($"Pool with name {name} does not exist.");
            return null;
        }

        ObjectPoolInfo currentPool = poolDictionary[name];

        if (currentPool.poolLength <= 0)
        {
            for (int i = 0; i < 5; i++) // 부족할 때 5개씩 추가 생성
            {
                GameObject obj = Instantiate(currentPool.obj, currentPool.parentObject);
                obj.SetActive(false);
                currentPool.Enqueue(obj);
            }
        }

        GameObject currentObject = currentPool.Dequeue();
        currentObject.transform.position = position;

        // 오브젝트가 이미 활성화된 상태인지 확인 후 활성화
        if (!currentObject.activeSelf)
        {
            currentObject.SetActive(true);
        }

        return currentObject;
    }

    public void ReturnToPool(string name, GameObject currentObject)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogError($"Pool with name {name} does not exist.");
            return;
        }

        ObjectPoolInfo pool = poolDictionary[name];

        // 오브젝트가 이미 비활성화된 상태인지 확인 후 비활성화
        if (currentObject.activeSelf)
        {
            currentObject.SetActive(false);
        }

        currentObject.transform.SetParent(pool.parentObject);
        pool.Enqueue(currentObject);
    }
}

