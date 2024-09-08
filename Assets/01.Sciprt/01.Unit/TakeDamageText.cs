// # System
using System.Collections;
using System.Collections.Generic;
using TMPro;

// # Unity
using UnityEngine;

public class TakeDamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI takeDamageText;
    [SerializeField] private float totalTime;
    [SerializeField] private float speed;
    [SerializeField] private float yOffset;

    [Header("TextAlpha")]
    [SerializeField] private Color alpha;
    [SerializeField] private float alphaSpeed;

    [Header("DestroyTiem")]
    [SerializeField] private float destroyTime;
    [SerializeField] private float currentDestoryTime;

    private void OnEnable()
    {
        takeDamageText = GetComponent<TextMeshProUGUI>();
        currentDestoryTime = destroyTime;
        alpha.a = 1f; // 알파 값을 1로 초기화
        transform.position += Vector3.up * yOffset;
        StartCoroutine(Co_WaitReturnPoolTime(destroyTime));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetText(int damage)
    {
        StartCoroutine(Co_DamageTMP(damage));
        takeDamageText.text = damage.ToString();
    }

    IEnumerator Co_WaitReturnPoolTime(float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPool.Instance.ReturnToPool("DamageText", gameObject);
    }

    IEnumerator Co_DamageTMP(int damageAmount)
    {
        while (true)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;

            alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
            takeDamageText.color = alpha;

            currentDestoryTime -= Time.deltaTime;

            if(currentDestoryTime <= 0)
            {
                ObjectPool.Instance.ReturnToPool("DamageText", gameObject);
                break;
            }

            yield return null;
        }
    }
}
