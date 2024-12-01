using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Icon
{
    public string iconName;
    public Sprite iconSprite;
    public int iconCount;
}

public class IconInfo
{
    public string iconName;
    public Sprite iconSprite;
    public GameObject iconBlack;
}

public class MiniGameManager : MonoBehaviour
{
    [Header("UI")]
    public List<IconInfo> iconInfos = new List<IconInfo>();
    private List<IconInfo> currentSelectIcon = new List<IconInfo>();
    [SerializeField] private GameObject btnPrefab;
    [SerializeField] private GameObject gridLayOutGroub;

    private void Start()
    {
        foreach (var icon in iconInfos)
        {
            
        }
    }

    public void SelectIcon(int index)
    {
        iconInfos[0].iconBlack.SetActive(false);
        currentSelectIcon.Add(iconInfos[index]);

        foreach(var curIcon in currentSelectIcon)
        {
            if (currentSelectIcon[0].iconName != curIcon.iconName)
            {
                foreach(var curIcon2 in currentSelectIcon)
                {
                    curIcon.iconBlack.SetActive(true);
                }
                currentSelectIcon.Clear();
                break;
            }

            if(currentSelectIcon.Count >= 3)
            {

            }
        }
    }
}
