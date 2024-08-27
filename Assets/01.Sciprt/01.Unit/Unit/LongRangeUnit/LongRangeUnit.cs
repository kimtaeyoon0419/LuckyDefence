// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
namespace LuckyDefence.Unit
{
    public class LongRangeUnit : Unit
    {
        [Header("Attack")]
        private bool isFire;
        [SerializeField] private GameObject bulletPrefab;

        [Header("Animation")]
        private Animator animator;
        private readonly int hashAttack = Animator.StringToHash("Attack");

        protected override void OnEnable()
        {
            base.OnEnable();
            animator = GetComponentInChildren<Animator>();
        }

        protected override void Attack()
        {
            Monster targetMonster = currentEnemy;

            if (targetMonster != null)
            {
                animator.SetTrigger(hashAttack);
                var unitBulltetObject = ObjectPool.Instance.SpawnFromPool("UnitBullet", transform.position);
                unitBulltetObject.GetComponent<Bullet>().SetupBullet(targetMonster, attackDamage);
            }
        }
    }
}
