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

    // 업그레이드 메서드
    public void UpGradeStat(string statName)
    {
        UpGradeStat stat = upGradeStats.Find(s => s.statName == statName);
        if (stat == null) return;

        if (!GoodsManager.Instance.UseGold(stat.upgradeCost).Item1)
        {
            return;
        }

        stat.level++;  // 레벨 증가
        stat.UpdateUI(); // UI 업데이트
    }

    #region Calc Methods
    public float CalcStat(string statName, float baseValue)
    {
        UpGradeStat stat = upGradeStats.Find(s => s.statName == statName);
        if (stat == null) return baseValue;

        return baseValue * (1 + stat.level * stat.multiplierPerLevel);
    }
    #endregion
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
        if (upgradeCostText != null) upgradeCostText.text = $"{upgradeCost}";
    }
}
