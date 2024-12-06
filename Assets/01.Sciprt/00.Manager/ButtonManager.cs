// # System
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;


// # Unity
using UnityEngine;

[Serializable]
public class Button
{
    public string btnName;
    public GameObject buttonObject;
    public GameObject blackColor;
    public GameObject page;
}

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private List<Button> buttonList;

    [Header("MiniGame")]
    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private float StartminiGameCoolTime;
    [SerializeField] private float currentMiniGameCoolTime;
    [SerializeField] private GameObject coolPanel;
    [SerializeField] private TextMeshProUGUI miniGameCoolText;

    private void Start()
    {
        SetButton("UnitSpawn");
    }

    private void Update()
    {
        if(currentMiniGameCoolTime > 0)
        {
            if(!coolPanel.activeSelf)   coolPanel.SetActive(true);
            currentMiniGameCoolTime -= Time.deltaTime;
            miniGameCoolText.text = Mathf.FloorToInt(currentMiniGameCoolTime).ToString();
        }
        else if(currentMiniGameCoolTime <= 0)
        {
            if (coolPanel.activeSelf) coolPanel.SetActive(false);
            currentMiniGameCoolTime = 0;
            miniGameCoolText.text = Mathf.FloorToInt(currentMiniGameCoolTime).ToString();
        }
    }

    public void SetButton(string buttonName)
    {
        foreach (var button in buttonList)
        {
            if(button.btnName != buttonName)
            {
                button.blackColor.SetActive(true);
                button.page.SetActive(false);
            }
            else
            {
                button.blackColor.SetActive(false);
                button.page.SetActive(true);
            }
        }
    }

    public void StartMiniGame()
    {
        if(currentMiniGameCoolTime > 0) return;
        miniGamePanel.SetActive(true);
        currentMiniGameCoolTime = StartminiGameCoolTime;
    }
}
