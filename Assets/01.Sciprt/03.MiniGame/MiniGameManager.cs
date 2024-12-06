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
    [SerializeField] private List<Icon> iconList = new List<Icon>();
    private List<Icon> iconListCopy = new List<Icon>(); // ���纻
    [SerializeField] private List<IconInfo> iconInfos = new List<IconInfo>();
    [SerializeField] private List<IconInfo> currentSelectIcon = new List<IconInfo>();
    [SerializeField] private GameObject btnPrefab;
    [SerializeField] private GameObject gridLayOutGroub;

    [Header("Time")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float startTime;
    [SerializeField] private float currentTime;

    [Header("Transition")]
    [SerializeField] private TransitionSettings transitionSettings;

    private void OnEnable()
    {
        // Ÿ�̸� �ʱ�ȭ
        currentTime = startTime;

        // ���� ������ ��ư ����
        foreach (var icon in iconInfos)
        {
            Destroy(icon.iconBtn);
        }
        iconInfos.Clear();

        // iconList ���纻 ����
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

        // ������ �ʱ�ȭ
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
                    // ��ư ����
                    GameObject btn = Instantiate(btnPrefab, transform.position, Quaternion.identity, gridLayOutGroub.transform);
                    btn.GetComponent<Image>().sprite = iconListCopy[ran].iconSprite;

                    // MiniGameBtn ��ũ��Ʈ�� ���� �� �г� ����
                    MiniGameBtn btnScript = btn.GetComponent<MiniGameBtn>();

                    // ������ ������ �����ϰ� iconInfos�� ����
                    IconInfo newIconInfo = new IconInfo
                    {
                        iconName = iconListCopy[ran].iconName,
                        iconSprite = iconListCopy[ran].iconSprite,
                        iconBtn = btn,
                        iconBlack = btnScript.blackPanel // MiniGameBtn���� �� �г� ��������
                    };
                    iconInfos.Add(newIconInfo);

                    // ��ư Ŭ�� �̺�Ʈ�� index ����
                    int currentIndex = iconInfos.Count - 1; // ���� �߰��� IconInfo�� index
                    btn.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners(); // ���� ������ ����
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
        IconInfo selectedIcon = iconInfos[index];

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
        foreach (IconInfo icon in currentSelectIcon)
        {
            icon.iconBlack.GetComponentInParent<UnityEngine.UI.Button>().interactable = false;
        }

        yield return new WaitForSeconds(0.5f);

        if (currentSelectIcon[0].iconName != currentSelectIcon[1].iconName)
        {
            Debug.Log("Ʋ��!");
            foreach (IconInfo curIcon in currentSelectIcon)
            {
                StartCoroutine(FadeInBlackPanel(curIcon.iconBlack));
            }
        }
        else
        {
            Debug.Log("��Ī ����!");
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
                //TransitionManager.Instance().Transition("StageScene", transitionSettings, 0);
                gameObject.SetActive(false);
            }
        }
    }
}
