// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
namespace LuckyDefence.Unit
{
    public class Knight : ShortRangeUnit
    {
        protected override void Attack()
        {
            base.Attack();
            //Debug.Log($"기사의 공격력 : {UpGradeManager.Instance.CalcAttackPower(attackDamage)}");
            AudioManager.Instance.StartSfx("Sword");
        }
    }
}
