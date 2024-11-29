// # System
using LuckyDefence.StatData;
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
using UnityEngine.UIElements;

namespace LuckyDefence.Unit
{
    public abstract class Unit : MonoBehaviour
    {
        [Header("Component")]
        protected Animator animator;

        [Header("Stat")]
        [SerializeField] protected string unitName;
        [SerializeField] protected Stat statData;
        protected float attackPower;
        protected float attackSpeed;
        protected float currentAttackSpeed;
        private float defaultAttackSpeed;
        protected float attackRange;
        protected float moveSpeed;

        [Header("CurrentEnemy")]
        [SerializeField] protected Monster currentEnemy;
        [SerializeField] protected LayerMask enemyLayer;

        [Header("CurrentState")]
        [SerializeField] protected bool isAttack = false;

        [Header("Animation")]
        protected readonly int hashAttack = Animator.StringToHash("Attack");
        protected readonly int hashMove = Animator.StringToHash("IsMove");

        [Header("Move")]
        [SerializeField] private bool isMove = false;

        protected virtual void OnEnable()
        {
            animator = GetComponentInChildren<Animator>();
            InitStat();
        }

        private void Update()
        {
            currentEnemy = FindEnemy();
            SetAttackSpeed();
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        protected void InitStat()
        {
            unitName = statData.unitName;
            attackPower = statData.damage;
            attackSpeed = statData.attackSpeed;
            attackRange = statData.attackRange;
            defaultAttackSpeed = attackSpeed;
            moveSpeed = statData.moveSpeed;
        }

        protected void SetAttackSpeed()
        {
            if (currentAttackSpeed <= 0)
            {
                if (currentEnemy != null && !isMove)
                {
                    if (defaultAttackSpeed != UpGradeManager.Instance.CalcStat("AttackSpeed", defaultAttackSpeed))
                    {
                        attackSpeed = UpGradeManager.Instance.CalcStat("AttackSpeed", defaultAttackSpeed);
                    }
                    currentAttackSpeed = attackSpeed;
                    Attack();
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
            if(MonsterSpawnManager.Instance.EnemyList.Count <= 0)
            {
                return null;
            }

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

        protected virtual void Attack()
        {
            Flip(currentEnemy.gameObject.transform);
        }

        private void Flip(Transform target)
        {
            if (target.transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (target.transform.position.x < transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        protected IEnumerator Co_WaitCurrentAnim()
        {
            yield return null;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
            isAttack = false;
        }

        public void MoveSlot(Transform postion)
        {
            isMove = true;
            Flip(postion);
            StartCoroutine(Co_MoveSlot(postion));
        }

        private IEnumerator Co_MoveSlot(Transform pos)
        {
            while (transform.position != pos.position)
            {
                animator.SetBool(hashMove, isMove);
                transform.position = Vector2.MoveTowards(transform.position, pos.position, moveSpeed * Time.deltaTime);
                yield return null;
            }
            isMove = false;
            animator.SetBool(hashMove, isMove);
        }
    }
}
