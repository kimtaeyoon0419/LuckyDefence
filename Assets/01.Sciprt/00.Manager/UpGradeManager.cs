using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpGradeManager : MonoBehaviour
{
    public static UpGradeManager Instance;

    // ���׷��̵� �׸� ����Ʈ
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

    // ���׷��̵� �޼���
    public void UpGradeStat(string statName)
    {
        UpGradeStat stat = upGradeStats.Find(s => s.statName == statName);
        if (stat == null) return;

        if (!GoodsManager.Instance.UseGold(stat.upgradeCost).Item1)
        {
            return;
        }

        stat.level++;  // ���� ����
        stat.UpdateUI(); // UI ������Ʈ
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

// ����ȭ�� ���� System.Serializable ��Ʈ����Ʈ �߰�
[System.Serializable]
public class UpGradeStat
{
    public string statName;  // ���� �̸�
    public int level;
    public float multiplierPerLevel;
    public int upgradeCost;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI upgradeCostText;

    // UI ������Ʈ �޼���
    public void UpdateUI()
    {
        if (levelText != null) levelText.text = $"Lv. {level}";
        if (upgradeCostText != null) upgradeCostText.text = $"{upgradeCost}";
    }
}
