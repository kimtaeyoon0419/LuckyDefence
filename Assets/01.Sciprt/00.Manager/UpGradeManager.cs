using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpGradeManager : MonoBehaviour
{
    [SerializeField] private float attackLevel;
    public float AttackLevel => attackLevel;

    [SerializeField] private float attackSpeedLevel;
    public float AttackSpeedLevel => attackSpeedLevel;

    [SerializeField] private float getMoneyLevel;
    public float GetMoneyLevel => getMoneyLevel;
}
