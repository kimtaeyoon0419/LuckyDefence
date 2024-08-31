using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using EasyTransition;

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
    [SerializeField] private bool isTitleDownEnd = false;
    [SerializeField] private TransitionSettings transitionSettings;

    private void Start()
    {
        touchToStartText.gameObject.SetActive(false);
        // �ΰ� ó���� ���� Ƣ�� ����� �ִϸ��̼�
        logoImage.transform.DOMoveY(targetPos.position.y, 1.2f).SetEase(initialEase).OnComplete(() =>
        {
            // �ִϸ��̼��� �Ϸ�� ��, ������ �������� ���Ʒ��� �ݺ������� ������
            logoImage.transform.DOLocalMoveY(logoImage.transform.localPosition.y + floatDistance, floatDuration)
                .SetEase(floatEase)
                .SetLoops(-1, LoopType.Yoyo); // ���� �ݺ�
            touchToStartText.gameObject.SetActive(true);
            touchToStartText.DOFade(1, 1).SetLoops(-1, LoopType.Yoyo);
            isTitleDownEnd = true;
        });
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && isTitleDownEnd)
        {
            TransitionManager.Instance().Transition("StageScene", transitionSettings, 0);
        }
    }
}
