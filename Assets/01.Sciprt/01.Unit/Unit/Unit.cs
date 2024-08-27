// # System
using LuckyDefence.StatData;
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

namespace LuckyDefence.Unit
{
    public abstract class Unit : MonoBehaviour
    {
        [Header("Stat")]
        [SerializeField] protected string unitName;
        [SerializeField] protected Stat statData;
        protected float attackDamage;
        protected float attackSpeed;
        protected float currentAttackSpeed;
        protected float attackRange;

        [Header("CurrentEnemy")]
        [SerializeField] protected Monster currentEnemy;
        [SerializeField] protected LayerMask enemyLayer;

        protected virtual void OnEnable()
        {
            InitStat();
        }

        private void Update()
        {
            currentEnemy = FindEnemy();
            SetAttackSpeed();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        protected void InitStat()
        {
            unitName = statData.unitName;
            attackDamage = statData.damage;
            attackSpeed = statData.attackSpeed;
            attackRange = statData.attackRange;
        }

        protected void SetAttackSpeed()
        {
            if (currentAttackSpeed <= 0)
            {
                if (currentEnemy != null)
                {
                    Attack();
                    currentAttackSpeed = attackSpeed;
                }
                else
                {
                    currentAttackSpeed = 0;
                }
            }
            else if (currentAttackSpeed > 0)
            {
                currentAttackSpeed -= Time.deltaTime;
            }
        }

        protected Monster FindEnemy()
        {
            Monster oldestEnemy = null;
            int oldestSpawnOrder = int.MaxValue;

            foreach (Monster enemy in MonsterSpawnManager.Instance.EnemyList)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                // 사거리 내에 있는지 확인
                if (distanceToEnemy <= attackRange)
                {
                    // 가장 먼저 스폰된 적을 선택
                    if (enemy.spawnOrder < oldestSpawnOrder)
                    {
                        oldestSpawnOrder = enemy.spawnOrder;
                        oldestEnemy = enemy;
                    }
                }
            }

            return oldestEnemy;
        }

        protected abstract void Attack();
    }
}
