// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
namespace LuckyDefence.Unit
{
    public class ShortRangeUnit : Unit
    {
        [Header("Attack")]
        [SerializeField] private GameObject attackPos;
        [SerializeField] private Vector2 attackBoxSize;

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackPos.transform.position, attackBoxSize);
        }

        protected override void Attack()
        {
            base.Attack();
            Monster targetMonster = currentEnemy;

            if (targetMonster != null && isAttack == false)
            {
                animator.ResetTrigger(hashAttack);
                animator.SetTrigger(hashAttack);
                //var unitBulltetObject = ObjectPool.Instance.SpawnFromPool("UnitBullet", transform.position);
                //unitBulltetObject.GetComponent<Bullet>().SetupBullet(targetMonster, attackDamage);
                attackPos.transform.position = targetMonster.transform.position;
                Collider2D[] col = Physics2D.OverlapBoxAll(attackPos.transform.position, attackBoxSize, 0);
                foreach(Collider2D col2d in col)
                {
                    col2d.GetComponent<Monster>()?.TakeDamage(attackDamage);
                }
            }
        }
    }
}
