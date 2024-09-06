using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpGradeManager : MonoBehaviour
{
    public static UpGradeManager Instance;

    [Header("AttackPower")]
    [SerializeField] private int attackLevel;
    public float AttackLevel => attackLevel;
    public float attackMultiplierPerLevel;
    public int attackLevelUpCost;

    [Header("AttackSpeed")]
    [SerializeField] private int attackSpeedLevel;
    public float AttackSpeedLevel => attackSpeedLevel;
    public float attackSpeedMultiplierPerLevel;
    public int attackSpeedLevelUpCost;

    [Header("GetMoney")]
    [SerializeField] private int getMoneyLevel;
    public float GetMoneyLevel => getMoneyLevel;
    public float getMoneyMultiplierPerLevel;
    public int getMoneyLevelUpCost;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region UpGrade
    public void UpGradeAttackPower()
    {
        if (GoodsManager.Instance.UseGold(attackLevelUpCost).Item1 == false)
        {
            return;
        }
        attackLevel++;
    }

    public void UpGradeAttackSpeed()
    {
        if (GoodsManager.Instance.UseGold(attackSpeedLevelUpCost).Item1 == false)
        {
            return;
        }
        attackSpeedLevel++;
    }

    public void UpGradeGetMoneyLevel()
    {
        if (GoodsManager.Instance.UseGold(getMoneyLevelUpCost).Item1 == false)
        {
            return;
        }
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
