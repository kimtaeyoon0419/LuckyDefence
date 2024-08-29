// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

namespace LuckyDefence.Unit
{
    public class Archer : LongRangeUnit
    {
        [SerializeField] private string sfxName;

        protected override void Attack()
        {
            base.Attack();
            AudioManager.Instance.StartSfx(sfxName);
        }
    }
}