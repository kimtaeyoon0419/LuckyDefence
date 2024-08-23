using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("WayPoints")]
    [SerializeField] private Transform[] wayPoints;
    private int wayIndex = 0;

    [Header("Stat")]
    [SerializeField] public float maxHp;
    [SerializeField] private float currentHp;

    [SerializeField]public float speed;
    [SerializeField]public float stoppingDistance = 0.1f;  // 목표 지점에 도달했다고 간주할 거리

    [SerializeField] private Canvas canvas;
    
    private void OnEnable()
    {
        StageManager.Instance.MonsterCountPlus();
        canvas = GetComponentInChildren<Canvas>();
        currentHp = maxHp;
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

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        TakeDamageText takeDamageText = ObjectPool.Instance.SpawnFromPool("DamageText", transform.position).GetComponent<TakeDamageText>();
        takeDamageText.gameObject.transform.SetParent(canvas.transform);
        takeDamageText.SetText(Mathf.FloorToInt(damage));
        if(currentHp <= 0)
        {
            SpawnManager.Instance.GetSp(10);
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
