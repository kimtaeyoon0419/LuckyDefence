// # System
using System;
using System.Collections;
using System.Collections.Generic;

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

    private void Start()
    {
        SetButton("UnitSpawn");
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
}
