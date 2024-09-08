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

    [SerializeField] public float speed;
    [SerializeField] public float stoppingDistance = 0.1f;  // 목표 지점에 도달했다고 간주할 거리

    [SerializeField] private Canvas canvas;

    private MonsterSpawnManager spawnManager;

    [SerializeField] public int spawnOrder;

    [SerializeField] private bool isDie = false;

    [SerializeField] private float dropGold;

    [Header("Animation")]
    private Animator animator;
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashDie = Animator.StringToHash("Die");

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        canvas = GetComponentInChildren<Canvas>();
    }

    private void OnEnable()
    {
        StageManager.Instance.MonsterCountPlus();
        currentHp = maxHp;
        wayIndex = 0;
        isDie = false;
        animator.ResetTrigger(hashDie);
    }

    private void OnDisable()
    {
        GoodsManager.Instance.GetGold(dropGold);
        StageManager.Instance.MonsterCountMinus();
    }

    private void Update()
    {
        if (currentHp > 0)
        {
            NextWayMove();
            if (transform.position.x > wayPoints[wayIndex].position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (transform.position.x < wayPoints[wayIndex].position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            animator.SetBool(hashDie, false);
        }
    }

    private void NextWayMove()
    {
        if (wayPoints == null || wayPoints.Length == 0) return;
        animator.SetBool(hashMove, true);

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

    public void Setup(MonsterSpawnManager monsterSpawnManager, Transform[] ways, int spawnCount)
    {
        spawnManager = monsterSpawnManager;
        wayPoints = new Transform[ways.Length];
        spawnOrder = spawnCount;

        for (int i = 0; i < ways.Length; i++)
        {
            wayPoints[i] = ways[i];
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentHp <= 0 || isDie) return;

        currentHp -= damage;

        TakeDamageText takeDamageText = ObjectPool.Instance.SpawnFromPool("DamageText", transform.position).GetComponent<TakeDamageText>();
        if (canvas.gameObject.activeInHierarchy)
        {
            takeDamageText.gameObject.transform.SetParent(canvas.transform);
        }

        takeDamageText.SetText(Mathf.FloorToInt(damage));
        if (currentHp <= 0 && !isDie)
        {
            Die();
        }
    }

    private void Die()
    {
        isDie = true;
        animator.SetBool(hashMove, false);
        StartCoroutine(Co_Die());
        UnitSlotManager.Instance.GetSp(5);
        GoodsManager.Instance.GetGold(10);
        //spawnManager.DestroyEnemy(this);
    }

    private IEnumerator Co_Die()
    {
        yield return new WaitForSeconds(0.01f);
        animator.SetTrigger(hashDie);
        yield return new WaitForSeconds(0.01f);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        spawnManager.DestroyEnemy(this);
    }
}
