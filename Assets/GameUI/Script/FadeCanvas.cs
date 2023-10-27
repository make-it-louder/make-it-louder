using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    private void Start()
    {
        // ������ �� ���� ���̵� ��
        StartCoroutine(FadeIn());
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float time = 0;
        Color startColor = fadeImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1);

        while (time < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = endColor;

        // �� ��ȯ
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeIn()
    {
        float time = 0;
        Color startColor = fadeImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        while (time < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = endColor;
    }
}
