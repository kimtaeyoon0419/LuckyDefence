// # System
using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using TMPro;

// # Unity
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("CurrentStage")]
    [SerializeField] private int currentStage;
    [SerializeField] private TextMeshProUGUI currentStageText;

    [Header("MonsterCount")]
    [SerializeField] private int monsterCount;
    [SerializeField] private TextMeshProUGUI monsterCountText;
    [SerializeField] public float maxMonsterCount;
 
    [Header("StageTime")]
    [SerializeField] private float stageTime;
    [SerializeField] private float curStageTime = 0;
    [SerializeField] private TextMeshProUGUI stageTimeText;

    [Header("GAME OVER")]
    private bool isGameOver;
    [SerializeField] private GameObject gameOverPaenl;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI stageCountText;
    [SerializeField] private TextMeshProUGUI touchToReturnTiteText;
    [SerializeField] private TransitionSettings transitionSettings;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        curStageTime = 3f;
    }

    private void Update()
    {
        if(curStageTime > 0 && !isGameOver)
        {
            curStageTime -= Time.deltaTime;
            stageTimeText.text = "00 : " + Mathf.FloorToInt(curStageTime).ToString("D2");
        }
        else if(curStageTime <= 0 && !isGameOver)
        {
            MonsterSpawnManager.Instance.StartSpawnMonster(20);
            curStageTime = stageTime;
            currentStage++;
            currentStageText.text = "현재 스테이지 : " + currentStage;

        }

        if(monsterCount >= maxMonsterCount && !isGameOver)
        {
            isGameOver = true;
            gameOverPaenl.SetActive(true);
            StartCoroutine(Co_GameOver());
        }

        monsterCountText.text = "몬스터 수 : " + monsterCount + " / " + maxMonsterCount;
    }

    public void UpDateMaxMonsterCount(float value)
    {
        maxMonsterCount = value;
    }

    public void MonsterCountPlus()
    {
        monsterCount++;
    }

    public void MonsterCountMinus()
    {
        monsterCount--;
        
        if(monsterCount <= 0 )
        {
            monsterCount = 0;
        }

    }

    IEnumerator Co_GameOver()
    {
        gameOverPaenl.SetActive(true);
        yield return new WaitForSeconds(1f);
        stageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        stageCountText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        for(int i = 0; i < currentStage; i++)
        {
            stageCountText.text = currentStage.ToString();
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.5f);
        touchToReturnTiteText.gameObject.SetActive(true);

        while(true)
        {
            if (Input.GetMouseButton(0))
            {
                TransitionManager.Instance().Transition("Title", transitionSettings, 0);
            }
            yield return null;
        }
    }
}
