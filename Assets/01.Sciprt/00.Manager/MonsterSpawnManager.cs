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

    [Header("SpawnDelay")]
    [SerializeField] private float spawnDelay;
    private WaitForSeconds waitSpawnDelay;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        waitSpawnDelay = new WaitForSeconds(spawnDelay);
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
            Instantiate(monster, spawnPos.position, Quaternion.identity).GetComponent<Monster>().SetWayPoint(wayPoints);
            spawnCount++;
            yield return waitSpawnDelay;
        }
    }
}
