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
        goldText.text = "현재 골드량 : " + gold;
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
    /// 골드 사용 함수 / 골드가 충족 = true / 골드가 부족 = false
    /// </summary>
    /// <param name="gold">사용할 골드의 양</param>
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
