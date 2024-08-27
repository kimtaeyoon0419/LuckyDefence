// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;
using LuckyDefence;

namespace LuckyDefence.StatData
{
    [CreateAssetMenu(fileName = "Archer", menuName = "Sciptable Object/Stat", order = int.MaxValue)]
    public class Stat : ScriptableObject
    {
        public string unitName;
        public float attackSpeed;
        public float damage;
        public float attackRange;
        public CharRating charRating;
    }
}
