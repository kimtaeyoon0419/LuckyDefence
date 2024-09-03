using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpGradeManager : MonoBehaviour
{
    [Header("AttackPower")]
    [SerializeField] private float attackLevel;
    public float AttackLevel => attackLevel;
    public float attackMultiplierPerLevel;
    public float attackLevelUpCost;

    [Header("AttackSpeed")]
    [SerializeField] private float attackSpeedLevel;
    public float AttackSpeedLevel => attackSpeedLevel;
    public float attackSpeedMultiplierPerLevel;
    public float attackSpeedLevelUpCost;

    [Header("GetMoney")]
    [SerializeField] private float getMoneyLevel;
    public float GetMoneyLevel => getMoneyLevel;
    public float getMoneyMultiplierPerLevel;
    public float getMoneyLevelUpCost;

    #region UpGrade
    public void UpGradeAttackPower()
    {
        attackLevel++;
    }

    public void UpGradeAttackSpeed()
    {
        attackSpeedLevel++;
    }

    public void UpGradeGetMoneyLevel()
    {
        getMoneyLevel++;
    }
    #endregion

    #region Calc
    /// <summary>
    /// 레벨 비례 공격력 계산
    /// </summary>
    /// <param name="attackPower">유닛의 공격력</param>
    /// <returns></returns>
    public float CalcAttackPower(float attackPower)
    {
        return attackPower * (1 + attackLevel * attackMultiplierPerLevel);
    }

    /// <summary>
    /// 레벨 비례 공격속도 계산
    /// </summary>
    /// <param name="attackSpeed">유닛의 공격속도</param>
    /// <returns></returns>
    public float CalcAttackSpeed(float attackSpeed)
    {
        return attackSpeed * (1 + attackSpeedLevel * attackSpeedMultiplierPerLevel);
    }

    public float CalcGetMoney(float getMoney)
    {
        return getMoney * (1 + getMoneyLevel * getMoneyMultiplierPerLevel);
    }
    #endregion
}
