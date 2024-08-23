// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    public static MonsterSpawnManager Instance;

    [Header("Waypoints")]
    [SerializeField] private Transform[] wayPoints;

    [Header("Monster")]
    [SerializeField] private GameObject monster;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float spawnTime;
    private float curSpawnTime = 0f;
    [SerializeField] private List<Monster> enemyList;
    public List<Monster> EnemyList => enemyList;
    private int spawnCount = 0;

    [Header("SpawnDelay")]
    [SerializeField] private float spawnDelay;
    private WaitForSeconds waitSpawnDelay;

    private void Awake()
    {
        Instance = this;
        enemyList = new List<Monster>();
    }

    private void Start()
    {
        waitSpawnDelay = new WaitForSeconds(spawnDelay);
    }

    private void Update()
    {
        if(enemyList.Count <= 0)
        {
            spawnCount = 0;
        }
    }

    public void StartSpawnMonster(int index)
    {
        StartCoroutine(Co_Spawn(index));
    }

    IEnumerator Co_Spawn(int index)
    {
        int spawnCount = 0;

        while (spawnCount < index)
        {
            GameObject clone = Instantiate(monster, spawnPos.position, Quaternion.identity);
            Monster enemy = clone.GetComponent<Monster>();
            enemy.Setup(this, wayPoints, spawnCount);
            spawnCount++;
            
            enemyList.Add(enemy);

            spawnCount++;
            yield return waitSpawnDelay;
        }
    }

    public void DestroyEnemy(Monster enemy)
    {
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}
