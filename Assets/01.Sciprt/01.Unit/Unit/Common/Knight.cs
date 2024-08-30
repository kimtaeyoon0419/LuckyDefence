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
            AudioManager.Instance.StartSfx("Sword");
        }
    }
}
