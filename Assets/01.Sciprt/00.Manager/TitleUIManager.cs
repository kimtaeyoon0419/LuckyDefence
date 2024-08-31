using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    [Header("Ui")]
    [SerializeField] private GameObject logoImage;
    [SerializeField] private Transform targetPos;
    [SerializeField] private float floatDistance = 30f; // 위아래로 움직일 거리
    [SerializeField] private float floatDuration = 1.5f; // 위아래로 움직일 시간
    [SerializeField] private Ease floatEase = Ease.InOutSine; // 부드러운 가속/감속을 위한 Ease 설정
    [SerializeField] private Ease initialEase; // 초기 튐 애니메이션을 위한 Ease 설정
    [SerializeField] private TextMeshProUGUI touchToStartText;

    private void Start()
    {
        touchToStartText.gameObject.SetActive(false);
        // 로고를 처음에 위로 튀게 만드는 애니메이션
        logoImage.transform.DOMoveY(targetPos.position.y, 1.2f).SetEase(initialEase).OnComplete(() =>
        {
            // 애니메이션이 완료된 후, 일정한 간격으로 위아래로 반복적으로 움직임
            logoImage.transform.DOLocalMoveY(logoImage.transform.localPosition.y + floatDistance, floatDuration)
                .SetEase(floatEase)
                .SetLoops(-1, LoopType.Yoyo); // 무한 반복
            touchToStartText.gameObject.SetActive(true);
            touchToStartText.DOFade(1, 1).SetLoops(-1, LoopType.Yoyo);
        });
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("StageScene");
        }
    }
}
