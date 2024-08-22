// # System
using System.Collections;
using System.Collections.Generic;
using TMPro;

// # Unity
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("MonsterCount")]
    [SerializeField] private int monsterCount;
    [SerializeField] private TextMeshProUGUI monsterCountText;

    private void Awake()
    {
        Instance = this;
    }

    public void MonsterCountPlus()
    {
        monsterCount++;
        monsterCountText.text = "몬스터 수 : " + monsterCount;
    }

    public void MonsterCountMinus()
    {
        monsterCount--;
        if(monsterCount <= 0 )
        {
            monsterCount = 0;
        }
        monsterCountText.text = "몬스터 수 : " + monsterCount;
    }
}
