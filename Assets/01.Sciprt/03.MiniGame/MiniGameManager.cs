using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private List<Icon> iconList = new List<Icon>();
    [SerializeField] private List<IconInfo> iconInfos = new List<IconInfo>();
    [SerializeField] private List<IconInfo> currentSelectIcon = new List<IconInfo>();
    [SerializeField] private GameObject btnPrefab;
    [SerializeField] private GameObject gridLayOutGroub;

    private void Start()
    {
        for (int i = 0; i < 16; i++)
        {
            while (true)
            {
                int ran = UnityEngine.Random.Range(0, iconList.Count);

                if (iconList[ran].iconCount > 0)
                {
                    // 버튼 생성
                    GameObject btn = Instantiate(btnPrefab, transform.position, Quaternion.identity, gridLayOutGroub.transform);
                    btn.GetComponent<Image>().sprite = iconList[ran].iconSprite;

                    // MiniGameBtn 스크립트를 통해 블랙 패널 관리
                    MiniGameBtn btnScript = btn.GetComponent<MiniGameBtn>();

                    // 아이콘 정보를 생성하고 iconInfos에 저장
                    IconInfo newIconInfo = new IconInfo
                    {
                        iconName = iconList[ran].iconName,
                        iconSprite = iconList[ran].iconSprite,
                        iconBtn = btn,
                        iconBlack = btnScript.blackPanel // MiniGameBtn에서 블랙 패널 가져오기
                    };
                    iconInfos.Add(newIconInfo);

                    // 버튼 클릭 이벤트에 index 전달
                    int currentIndex = iconInfos.Count - 1; // 현재 추가된 IconInfo의 index
                    btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectIcon(currentIndex));

                    iconList[ran].iconCount--;
                    break;
                }
            }
        }
    }

    public void SelectIcon(int index) // 아이콘 선택
    { 

        IconInfo selectedIcon = iconInfos[index];

        if (selectedIcon.iconBtn.GetComponent<MiniGameBtn>().isComplete) return;

        // 선택된 아이콘 가림판 제거
        MiniGameBtn btnScript = selectedIcon.iconBlack.GetComponentInParent<MiniGameBtn>();
        if (btnScript != null)
        {
            btnScript.blackPanel.SetActive(false);
        }

        currentSelectIcon.Add(selectedIcon);

        if (currentSelectIcon.Count == 2)
        {
            // 클릭 딜레이를 위해 코루틴 시작
            StartCoroutine(CheckIconsWithDelay());
        }
    }

    private IEnumerator CheckIconsWithDelay()
    {
        // 딜레이 동안 다음 클릭 비활성화
        foreach (IconInfo icon in currentSelectIcon)
        {
            icon.iconBlack.GetComponentInParent<UnityEngine.UI.Button>().interactable = false;
        }

        yield return new WaitForSeconds(0.5f); // 딜레이 시간

        if (currentSelectIcon[0].iconName != currentSelectIcon[1].iconName)
        {
            Debug.Log("틀림!");
            foreach (IconInfo curIcon in currentSelectIcon)
            {
                // 블랙 패널 페이드 효과와 함께 활성화
                StartCoroutine(FadeInBlackPanel(curIcon.iconBlack));
            }
        }
        else
        {
            Debug.Log("매칭 성공!");

            foreach(IconInfo curIcon in currentSelectIcon)
            {
                curIcon.iconBtn.GetComponent<MiniGameBtn>().isComplete = true;
            }
            // 매칭 성공 처리
        }

        currentSelectIcon.Clear();

        // 다시 클릭 활성화
        foreach (IconInfo icon in iconInfos)
        {
            icon.iconBlack.GetComponentInParent<UnityEngine.UI.Button>().interactable = true;
        }
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
            canvasGroup.alpha += Time.deltaTime * 2f; // 페이드 속도 조정
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
