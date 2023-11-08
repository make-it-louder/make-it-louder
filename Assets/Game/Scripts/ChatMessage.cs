using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class ChatMessage : MonoBehaviour
{
    private TextMeshProUGUI tmpText;
    private RectTransform rectTransform;
    public float fadeDelay = 2.0f; // 페이드 아웃이 시작되기 전의 지연 시간 (초)
    public float fadeDuration = 2.0f; // 페이드 아웃 지속 시간 (초)
    private bool isFadingOut = false;
    private float saveAlpha = 1.0f;
    private float savetime = 0.0f;
    private Coroutine myCoroutine;
    private bool stop = false;
    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        isFadingOut = true;
    }
    private void Start()
    {
        myCoroutine = StartCoroutine(FadeOut(fadeDelay, fadeDuration));
    }
    // 이 메서드를 호출하면 코루틴이 중지됩니다.
    public void StopMyCoroutine()
    {
        if (myCoroutine != null)
        {
            Debug.Log("스탑코루틴!");
            StopCoroutine(myCoroutine);
            myCoroutine = null; // 코루틴 참조를 제거합니다.
            stop = true;
        }
    }

    // 이 메서드를 호출하면 코루틴이 다시 시작됩니다.
    public void RestartMyCoroutine()
    {
        if (myCoroutine == null) // 현재 실행 중인 코루틴이 없을 때만 다시 시작합니다.
        {
            if (isFadingOut)
            {
                stop = false;
                myCoroutine = StartCoroutine(FadeOut(fadeDelay, fadeDuration));
            }
        }
    }
    public void SetText()
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, tmpText.preferredHeight);
    }
    IEnumerator FadeOut(float delay, float duration)
    {
        Debug.Log("코루틴!");
        // 현재 알파값에서 시작합니다.
        float startAlpha = saveAlpha;
        float time = savetime;

        while (time < (delay + duration))
        {
            if (stop) // 플래그 상태를 확인하여 코루틴 중지 여부 결정
            {
                yield break; // 코루틴을 즉시 종료합니다.
            }
            savetime += Time.deltaTime;
            time = savetime;
            if (time > delay)
            {
                // 시간에 따라 알파값을 감소시킵니다.
                saveAlpha = Mathf.Lerp(startAlpha, 0, ((time - delay) / duration));
                tmpText.alpha = saveAlpha;
            }
            yield return null; // 다음 프레임까지 기다립니다.

        }

        // 최종 알파값을 0으로 설정하여 완전히 투명하게 만듭니다.
        tmpText.alpha = 0;
        isFadingOut = false;
    }

    public bool IsFadeOutRunning()
    {
        return isFadingOut;
    }
}
