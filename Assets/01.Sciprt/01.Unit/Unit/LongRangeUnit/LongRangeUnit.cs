// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class LongRangeUnit : Unit
{
    [Header("Attack")]
    private bool isFire;
    [SerializeField] private GameObject bulletPrefab;

    protected override void Attack()
    {
        Monster targetMonster = currentEnemy;

        if (targetMonster != null)
        {
            var unitBulltetObject = ObjectPool.Instance.SpawnFromPool("UnitBullet", transform.position);
            unitBulltetObject.GetComponent<Bullet>().SetupBullet(targetMonster, attackDamage);
        }
    }
}
