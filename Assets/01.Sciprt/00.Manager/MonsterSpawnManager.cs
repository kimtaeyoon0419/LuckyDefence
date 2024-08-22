// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private Transform[] wayPoints;

    [Header("Monster")]
    [SerializeField] private GameObject monster;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float spawnTime;
    private float curSpawnTime = 0f;

    private void Update()
    {
        if (0 >= curSpawnTime)
        {
            curSpawnTime = spawnTime;
            Instantiate(monster, spawnPos.position, Quaternion.identity).GetComponent<Monster>().SetWayPoint(wayPoints);
        }
        else if (spawnTime >= curSpawnTime)
        {
            //Debug.Log("∞®º“¡ﬂ");
            curSpawnTime -= Time.deltaTime;
        }
    }
}
