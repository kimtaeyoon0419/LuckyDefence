using EasyTransition;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Icon
{
    public string iconName;
    public Sprite iconSprite;
    public int iconCount;
}

[System.Serializable]
public class IconInfo
{
    public string iconName;
    public Sprite iconSprite;
    public GameObject iconBtn;
    public GameObject iconBlack;
}

public class MiniGameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject miniGameCanvas;
    [SerializeField] private List<Icon> iconList = new List<Icon>();
    private List<Icon> iconListCopy = new List<Icon>(); // 복사본
    [SerializeField] private List<IconInfo> iconInfos = new List<IconInfo>();
    [SerializeField] private List<IconInfo> currentSelectIcon = new List<IconInfo>();
    [SerializeField] private GameObject btnPrefab;
    [SerializeField] private GameObject gridLayOutGroub;
    [SerializeField] private TextMeshProUGUI countText;

    [Header("Time")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float startTime;
    [SerializeField] private float currentTime;

    [Header("Transition")]
    [SerializeField] private TransitionSettings transitionSettings;

    [Header("Count")]
    [SerializeField] private int completeCount = 0;

    private bool isProcessing = false; // 선택 중인지 확인하는 플래그

    [Header("ResultText")]
    [SerializeField] private GameObject resultText;

    private void OnEnable()
    {
        // 타이머 초기화
        currentTime = startTime;
        completeCount = 0;
        countText.text = completeCount.ToString() + "개를 찾았습니다!";

        // 기존 아이콘 버튼 제거
        foreach (var icon in iconInfos)
        {
            Destroy(icon.iconBtn);
        }
        iconInfos.Clear();

        // iconList 복사본 생성
        iconListCopy.Clear();
        foreach (var icon in iconList)
        {
            iconListCopy.Add(new Icon
            {
                iconName = icon.iconName,
                iconSprite = icon.iconSprite,
                iconCount = icon.iconCount
            });
        }

        // 아이콘 초기화
        StartCoroutine(SetupIcons());
    }

    private IEnumerator SetupIcons()
    {
        for (int i = 0; i < 16; i++)
        {
            while (true)
            {
                int ran = UnityEngine.Random.Range(0, iconListCopy.Count);

                if (iconListCopy[ran].iconCount > 0)
                {
                    // 버튼 생성
                    GameObject btn = Instantiate(btnPrefab, transform.position, Quaternion.identity, gridLayOutGroub.transform);
                    btn.GetComponent<Image>().sprite = iconListCopy[ran].iconSprite;

                    // MiniGameBtn 스크립트를 통해 블랙 패널 관리
                    MiniGameBtn btnScript = btn.GetComponent<MiniGameBtn>();

                    // 아이콘 정보를 생성하고 iconInfos에 저장
                    IconInfo newIconInfo = new IconInfo
                    {
                        iconName = iconListCopy[ran].iconName,
                        iconSprite = iconListCopy[ran].iconSprite,
                        iconBtn = btn,
                        iconBlack = btnScript.blackPanel // MiniGameBtn에서 블랙 패널 가져오기
                    };
                    iconInfos.Add(newIconInfo);

                    // 버튼 클릭 이벤트에 index 전달
                    int currentIndex = iconInfos.Count - 1; // 현재 추가된 IconInfo의 index
                    btn.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners(); // 기존 리스너 제거
                    btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectIcon(currentIndex));

                    iconListCopy[ran].iconCount--;
                    break;
                }
            }
        }

        yield return null;
    }

    private void Update()
    {
        if (this.gameObject.activeSelf)
            TimeFunc();
    }

    public void SelectIcon(int index)
    {
        if (isProcessing) return; // 현재 처리 중이면 입력 막기

        IconInfo selectedIcon = iconInfos[index];

        // 이미 선택된 아이콘인지 확인
        if (currentSelectIcon.Contains(selectedIcon)) return;

        if (selectedIcon.iconBtn.GetComponent<MiniGameBtn>().isComplete) return;

        MiniGameBtn btnScript = selectedIcon.iconBlack.GetComponentInParent<MiniGameBtn>();
        if (btnScript != null)
        {
            btnScript.blackPanel.SetActive(false);
        }

        currentSelectIcon.Add(selectedIcon);

        if (currentSelectIcon.Count == 2)
        {
            StartCoroutine(CheckIconsWithDelay());
        }
    }

    private IEnumerator CheckIconsWithDelay()
    {
        isProcessing = true; // 선택 처리 시작
        foreach (IconInfo icon in currentSelectIcon)
        {
            icon.iconBlack.GetComponentInParent<UnityEngine.UI.Button>().interactable = false;
        }

        yield return new WaitForSeconds(0.5f);

        if (currentSelectIcon[0].iconName != currentSelectIcon[1].iconName)
        {
            Debug.Log("틀림!");
            foreach (IconInfo curIcon in currentSelectIcon)
            {
                StartCoroutine(FadeInBlackPanel(curIcon.iconBlack));
            }
        }
        else
        {
            Debug.Log("매칭 성공!");
            completeCount++;
            countText.text = completeCount.ToString() + "개를 찾았습니다!";
            foreach (IconInfo curIcon in currentSelectIcon)
            {
                curIcon.iconBtn.GetComponent<MiniGameBtn>().isComplete = true;
            }
        }

        currentSelectIcon.Clear();

        foreach (IconInfo icon in iconInfos)
        {
            icon.iconBlack.GetComponentInParent<UnityEngine.UI.Button>().interactable = true;
        }

        isProcessing = false; // 처리 완료
    }

    private IEnumerator FadeInBlackPanel(GameObject blackPanel)
    {
        CanvasGroup canvasGroup = blackPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = blackPanel.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        blackPanel.SetActive(true);

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * 2f;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private void TimeFunc()
    {
        if (currentTime >= 0)
        {
            currentTime -= Time.deltaTime;
            timeText.text = Mathf.FloorToInt(currentTime).ToString();
        }

        if (currentTime < 0f)
        {
            if (TransitionManager.Instance() != null)
            {
                GoodsManager.Instance.GetGold(10 * completeCount);
                resultText.GetComponent<MiniGameResultText>().SetText(completeCount, UpGradeManager.Instance.CalcStat("GetMoney", (float)(10 * completeCount)));
                miniGameCanvas.SetActive(false);
            }
        }
    }
}