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
        [SerializeField] private GameObject bulletPrefab;

        protected override void Attack()
        {
            Monster targetMonster = currentEnemy;

            if (targetMonster != null && isAttack == false)
            {
                animator.ResetTrigger(hashAttack);
                animator.SetTrigger(hashAttack);
                var unitBulltetObject = ObjectPool.Instance.SpawnFromPool("UnitBullet", transform.position);
                unitBulltetObject.GetComponent<Bullet>().SetupBullet(targetMonster, attackDamage);
            }
        }
    }
}
