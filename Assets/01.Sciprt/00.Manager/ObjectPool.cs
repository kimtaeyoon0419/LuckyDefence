using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolInfo
{
    public string objectName;      // Ǯ�� �̸�
    public GameObject obj;         // Ǯ���� �⺻ ������Ʈ ������
    public int initialPoolSize = 10;  // �ʱ� ������ ������Ʈ ��

    public int poolLength => pool.Count;  // ���� ť�� �ִ� ������Ʈ ��

    public Transform parentObject;  // Ǯ�� �θ� ������Ʈ (Hierarchy ������)

    private Queue<GameObject> pool = new Queue<GameObject>();

    // ������Ʈ ť�� �߰�
    public void Enqueue(GameObject _object) => pool.Enqueue(_object);

    // ������Ʈ ť���� ����
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
            for (int i = 0; i < 5; i++) // ������ �� 5���� �߰� ����
            {
                GameObject obj = Instantiate(currentPool.obj, currentPool.parentObject);
                obj.SetActive(false);
                currentPool.Enqueue(obj);
            }
        }

        GameObject currentObject = currentPool.Dequeue();
        currentObject.transform.position = position;

        // ������Ʈ�� �̹� Ȱ��ȭ�� �������� Ȯ�� �� Ȱ��ȭ
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

        // ������Ʈ�� �̹� ��Ȱ��ȭ�� �������� Ȯ�� �� ��Ȱ��ȭ
        if (currentObject.activeSelf)
        {
            currentObject.SetActive(false);
        }

        currentObject.transform.SetParent(pool.parentObject);
        pool.Enqueue(currentObject);
    }
}

