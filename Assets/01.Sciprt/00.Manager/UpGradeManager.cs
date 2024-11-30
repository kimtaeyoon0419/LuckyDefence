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

    /// <summary>
    /// ���׷��̵� �Լ�
    /// </summary>
    /// <param name="statName">���׷��̵��� ������ �̸�</param>
    public void UpGradeStat(string statName)
    {
        UpGradeStat stat = upGradeStats.Find(s => s.statName == statName);
        if (stat == null) return;

        if (!GoodsManager.Instance.UseGold(stat.upgradeCost).Item1)
        {
            StartCoroutine(Co_LowMoneyText(stat.upgradeCostText));
            return;
        }

        stat.level++;  // ���� ����
        stat.UpdateUI(); // UI ������Ʈ

        if(statName == "MaxMonsterCount")
        {
            StageManager.Instance.UpDateMaxMonsterCount(CalcMaxMonsterCountStat(StageManager.Instance.maxMonsterCount));
        }
    }

    #region Calc Methods
    /// <summary>
    /// ������ ������ ������ִ� �Լ�
    /// </summary>
    /// <param name="statName">����� ���� �̸�</param>
    /// <param name="baseValue">���� ��</param>
    /// <returns></returns>
    public float CalcStat(string statName, float baseValue)
    {
        UpGradeStat stat = upGradeStats.Find(s => s.statName == statName);
        if (stat == null) return baseValue;

        if (statName == "AttackSpeed")
        {
            // ���� �ӵ��� ������ �ö󰥼��� ���� (�� ������ ����)
            return baseValue / (1 + stat.level * stat.multiplierPerLevel);
        }
        else
        {
            // �⺻ ��� ��� (���� ����)
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
        upText.text = "���� �����մϴ�.";
        yield return new WaitForSeconds(0.2f);
        upText.text = textData;
    }
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
        if (upgradeCostText != null) upgradeCostText.text = $"{upgradeCost} ��";
    }
}
