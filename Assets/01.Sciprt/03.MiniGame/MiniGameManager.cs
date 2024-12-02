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
                    // ��ư ����
                    GameObject btn = Instantiate(btnPrefab, transform.position, Quaternion.identity, gridLayOutGroub.transform);
                    btn.GetComponent<Image>().sprite = iconList[ran].iconSprite;

                    // MiniGameBtn ��ũ��Ʈ�� ���� �� �г� ����
                    MiniGameBtn btnScript = btn.GetComponent<MiniGameBtn>();

                    // ������ ������ �����ϰ� iconInfos�� ����
                    IconInfo newIconInfo = new IconInfo
                    {
                        iconName = iconList[ran].iconName,
                        iconSprite = iconList[ran].iconSprite,
                        iconBtn = btn,
                        iconBlack = btnScript.blackPanel // MiniGameBtn���� �� �г� ��������
                    };
                    iconInfos.Add(newIconInfo);

                    // ��ư Ŭ�� �̺�Ʈ�� index ����
                    int currentIndex = iconInfos.Count - 1; // ���� �߰��� IconInfo�� index
                    btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectIcon(currentIndex));

                    iconList[ran].iconCount--;
                    break;
                }
            }
        }
    }

    public void SelectIcon(int index) // ������ ����
    { 

        IconInfo selectedIcon = iconInfos[index];

        if (selectedIcon.iconBtn.GetComponent<MiniGameBtn>().isComplete) return;

        // ���õ� ������ ������ ����
        MiniGameBtn btnScript = selectedIcon.iconBlack.GetComponentInParent<MiniGameBtn>();
        if (btnScript != null)
        {
            btnScript.blackPanel.SetActive(false);
        }

        currentSelectIcon.Add(selectedIcon);

        if (currentSelectIcon.Count == 2)
        {
            // Ŭ�� �����̸� ���� �ڷ�ƾ ����
            StartCoroutine(CheckIconsWithDelay());
        }
    }

    private IEnumerator CheckIconsWithDelay()
    {
        // ������ ���� ���� Ŭ�� ��Ȱ��ȭ
        foreach (IconInfo icon in currentSelectIcon)
        {
            icon.iconBlack.GetComponentInParent<UnityEngine.UI.Button>().interactable = false;
        }

        yield return new WaitForSeconds(0.5f); // ������ �ð�

        if (currentSelectIcon[0].iconName != currentSelectIcon[1].iconName)
        {
            Debug.Log("Ʋ��!");
            foreach (IconInfo curIcon in currentSelectIcon)
            {
                // �� �г� ���̵� ȿ���� �Բ� Ȱ��ȭ
                StartCoroutine(FadeInBlackPanel(curIcon.iconBlack));
            }
        }
        else
        {
            Debug.Log("��Ī ����!");

            foreach(IconInfo curIcon in currentSelectIcon)
            {
                curIcon.iconBtn.GetComponent<MiniGameBtn>().isComplete = true;
            }
            // ��Ī ���� ó��
        }

        currentSelectIcon.Clear();

        // �ٽ� Ŭ�� Ȱ��ȭ
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
            canvasGroup.alpha += Time.deltaTime * 2f; // ���̵� �ӵ� ����
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
