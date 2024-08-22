using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("WayPoints")]
    [SerializeField] private Transform[] wayPoints;
    private int wayIndex = 0;

    [Header("Stat")]
    public float speed;
    public float stoppingDistance = 0.1f;  // 목표 지점에 도달했다고 간주할 거리

    private void OnEnable()
    {
        StageManager.Instance.MonsterCountPlus();
    }

    private void OnDestroy()
    {
        StageManager.Instance.MonsterCountMinus();
    }

    private void Update()
    {
        NextWayMove();
    }

    private void NextWayMove()
    {
        if (wayPoints == null || wayPoints.Length == 0) return;

        // 목표 지점까지의 방향 계산
        Vector3 direction = wayPoints[wayIndex].position - transform.position;
        float distance = direction.magnitude;

        // 정규화된 방향 벡터 계산 (magnitude가 0이 아닌 경우에만)
        if (distance > stoppingDistance)
        {
            Vector3 move = direction.normalized * speed * Time.deltaTime;
            transform.position += move;
        }

        // 목표 지점에 도달했는지 체크
        if (distance <= stoppingDistance)
        {
            // 다음 지점으로 인덱스 이동 (순환)
            wayIndex = (wayIndex + 1) % wayPoints.Length;
        }
    }

    public void SetWayPoint(Transform[] ways)
    {
        wayPoints = new Transform[ways.Length];
        for (int i = 0; i < ways.Length; i++)
        {
            wayPoints[i] = ways[i];
        }
    }
}
