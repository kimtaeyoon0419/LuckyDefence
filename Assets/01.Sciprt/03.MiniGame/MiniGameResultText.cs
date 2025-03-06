using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGameResultText : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        SetFalse();
    }

    private void OnDisable()
    {
        text.text = "";
    }

    public void SetFalse()
    {
        gameObject.SetActive(false);
    }

    public void SetText(float checkCount, float moneyValue)
    {
        text.text = $"미니게임에서 {checkCount}개수만큼 성공하여 {moneyValue}만큼 골드를 획득하였습니다.";
        gameObject.SetActive(true);
    }
}
