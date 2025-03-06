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
        text.text = $"�̴ϰ��ӿ��� {checkCount}������ŭ �����Ͽ� {moneyValue}��ŭ ��带 ȹ���Ͽ����ϴ�.";
        gameObject.SetActive(true);
    }
}
