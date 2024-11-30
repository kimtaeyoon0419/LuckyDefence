using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpGradeManager : MonoBehaviour
{
    public static UpGradeManager Instance;

    // 업그레이드 항목 리스트
    [SerializeField] private List<UpGradeStat> upGradeStats;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 업그레이드 함수
    /// </summary>
    /// <param name="statName">업그레이드할 스탯의 이름</param>
    public void UpGradeStat(string statName)
    {
        UpGradeStat stat = upGradeStats.Find(s => s.statName == statName);
        if (stat == null) return;

        if (!GoodsManager.Instance.UseGold(stat.upgradeCost).Item1)
        {
            StartCoroutine(Co_LowMoneyText(stat.upgradeCostText));
            return;
        }

        stat.level++;  // 레벨 증가
        stat.UpdateUI(); // UI 업데이트

        if(statName == "MaxMonsterCount")
        {
            StageManager.Instance.UpDateMaxMonsterCount(CalcMaxMonsterCountStat(StageManager.Instance.maxMonsterCount));
        }
    }

    #region Calc Methods
    /// <summary>
    /// 스탯의 배율을 계산해주는 함수
    /// </summary>
    /// <param name="statName">사용할 스탯 이름</param>
    /// <param name="baseValue">기초 값</param>
    /// <returns></returns>
    public float CalcStat(string statName, float baseValue)
    {
        UpGradeStat stat = upGradeStats.Find(s => s.statName == statName);
        if (stat == null) return baseValue;

        if (statName == "AttackSpeed")
        {
            // 공격 속도는 레벨이 올라갈수록 감소 (더 빠르게 공격)
            return baseValue / (1 + stat.level * stat.multiplierPerLevel);
        }
        else
        {
            // 기본 계산 방식 (스탯 증가)
            return baseValue * (1 + stat.level * stat.multiplierPerLevel);
        }
    }


    public float CalcMaxMonsterCountStat(float baseValue)
    {
        UpGradeStat stat = upGradeStats.Find(s => s.statName == "MaxMonsterCount");
        if (stat == null) return baseValue;

        return baseValue +  10;
    }
    #endregion

    IEnumerator Co_LowMoneyText(TextMeshProUGUI upText)
    {
        string textData = upText.text;
        upText.text = "돈이 부족합니다.";
        yield return new WaitForSeconds(0.2f);
        upText.text = textData;
    }
}

// 직렬화를 위해 System.Serializable 어트리뷰트 추가
[System.Serializable]
public class UpGradeStat
{
    public string statName;  // 스탯 이름
    public int level;
    public float multiplierPerLevel;
    public int upgradeCost;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI upgradeCostText;

    // UI 업데이트 메서드
    public void UpdateUI()
    {
        if (levelText != null) levelText.text = $"Lv. {level}";
        if (upgradeCostText != null) upgradeCostText.text = $"{upgradeCost} 원";
    }
}
