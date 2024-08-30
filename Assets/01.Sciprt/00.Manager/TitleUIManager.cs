using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TitleUIManager : MonoBehaviour
{
    [Header("Ui")]
    [SerializeField] private GameObject logoImage;
    [SerializeField] private Transform targetPos;
    [SerializeField] private float floatDistance = 30f; // ���Ʒ��� ������ �Ÿ�
    [SerializeField] private float floatDuration = 1.5f; // ���Ʒ��� ������ �ð�
    [SerializeField] private Ease floatEase = Ease.InOutSine; // �ε巯�� ����/������ ���� Ease ����
    [SerializeField] private Ease initialEase; // �ʱ� Ʀ �ִϸ��̼��� ���� Ease ����
    [SerializeField] private TextMeshProUGUI touchToStartText;

    private void Start()
    {
        // �ΰ� ó���� ���� Ƣ�� ����� �ִϸ��̼�
        logoImage.transform.DOMoveY(targetPos.position.y, 1.2f).SetEase(initialEase).OnComplete(() =>
        {
            // �ִϸ��̼��� �Ϸ�� ��, ������ �������� ���Ʒ��� �ݺ������� ������
            logoImage.transform.DOLocalMoveY(logoImage.transform.localPosition.y + floatDistance, floatDuration)
                .SetEase(floatEase)
                .SetLoops(-1, LoopType.Yoyo); // ���� �ݺ�
        });
    }
}
