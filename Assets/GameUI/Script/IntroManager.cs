using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoFade : MonoBehaviour
{
    public Image firstLogo; // Drag your first logo here
    public Image secondLogo; // Drag your second logo here
    public float seconds;
    public float textSeconds;
    public TMP_Text pressAny;
    bool isOkay = false;
    private void Start()
    {
        StartCoroutine(FadeLogos());
    }

    private void Update()
    {
        if (Input.anyKey & isOkay)
        {
            SceneManager.LoadScene("Login");
        }
    }
    IEnumerator FadeLogos()
    {
        yield return StartCoroutine(FadeInOut(firstLogo));
        yield return StartCoroutine(FadeIn(secondLogo));
        yield return StartCoroutine(TextAnimation(pressAny));


    }

    IEnumerator FadeInOut(Image logoImage)
    {
        // Fade in
        for (float i = 0; i <= 1; i += Time.deltaTime / seconds)
        {
            Color color = logoImage.color;
            color.a = i;
            logoImage.color = color;
            yield return null;
        }

        // Fade out
        for (float i = 1; i >= 0; i -= Time.deltaTime / seconds)
        {
            Color color = logoImage.color;
            color.a = i;
            logoImage.color = color;
            yield return null;
        }
    }

    IEnumerator FadeIn(Image logoImage)

    {
        for (float i = 0; i <= 1; i += Time.deltaTime / 3)
        {
            Color color = logoImage.color;
            color.a = i;
            logoImage.color = color;
            yield return null;
        }
        Color lastColor = logoImage.color;
        lastColor.a = 1;
        isOkay = true;
    }

    IEnumerator TextAnimation(TMP_Text text)
    {
        while (true) { 
        // Fade in
            for (float i = 0; i <= 1; i += Time.deltaTime / textSeconds)
            {
                Color color = text.color;
                color.a = i;
                text.color = color;
                yield return null;
            }

            // Fade out
            for (float i = 1; i >= 0; i -= Time.deltaTime / textSeconds)
            {
                Color color = text.color;
                color.a = i;
                text.color = color;
                yield return null;
            }
        }
    }


}
