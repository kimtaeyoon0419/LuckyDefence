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

    [Header("StageTime")]
    [SerializeField] private float stageTime;
    [SerializeField] private float curStageTime = 0;
    [SerializeField] private TextMeshProUGUI stageTimeText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        curStageTime = stageTime;
    }

    private void Update()
    {
        if(curStageTime > 0)
        {
            curStageTime -= Time.deltaTime;
            stageTimeText.text = "00 : " + Mathf.FloorToInt(curStageTime).ToString("D2");
        }
        else if(curStageTime <= 0)
        {
            MonsterSpawnManager.Instance.StartSpawnMonster(20);
            curStageTime = stageTime;
        }
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
