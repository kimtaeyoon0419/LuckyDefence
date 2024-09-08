using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoodsManager : MonoBehaviour
{
    public static GoodsManager Instance;

    [SerializeField] private float gold;
    [SerializeField] private TextMeshProUGUI goldText;


    [SerializeField] private int sp;

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

    private void Update()
    {
        goldText.text = "���� ��差 : " + gold;
    }

    public void GetGold(float gold)
    {
        this.gold += UpGradeManager.Instance.CalcStat("GetMoney", gold);
    }

    public void GetSp(int sp)
    {
        this.sp += sp;
    }

    /// <summary>
    /// ��� ��� �Լ� / ��尡 ���� = true / ��尡 ���� = false
    /// </summary>
    /// <param name="gold">����� ����� ��</param>
    /// <returns></returns>
    public (bool, float? value) UseGold(float gold)
    {
        if (this.gold < gold)
        {
            return (false, null);
        }
        else
        {
            this.gold -= gold;
            return (true, gold);
        }
    }
}
