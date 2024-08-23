using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsManager : MonoBehaviour
{
    public static GoodsManager Instance;

    [SerializeField] private int gold;

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

    public void GetGold(int gold)
    {
        this.gold += gold;
    }

    public void GetSp(int sp)
    {
        this.sp += sp;
    }

    public (bool, int? value) UseGold(int gold)
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
