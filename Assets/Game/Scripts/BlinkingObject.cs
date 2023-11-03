using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingObject : MonoBehaviour
{
    public float blinkInterval = 0.5f;  // Set the blink interval in seconds
    public float minAlpha = 0.2f;  // Set the minimum alpha value
    public float maxAlpha = 1f;  // Set the maximum alpha value

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private bool isBlinking;

    void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void StartBlinking()
    {
        isBlinking = true;
        StartCoroutine(Blink());
    }

    public void StopBlinking()
    {
        isBlinking = false;
        StopCoroutine(Blink());
        SetAlpha(maxAlpha);
    }

    IEnumerator Blink()
    {
        while (isBlinking)
        {
            Debug.Log("블링크!!!!");
            SetAlpha(maxAlpha);
            yield return new WaitForSeconds(blinkInterval);
            SetAlpha(minAlpha);
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    void SetAlpha(float alpha)
    {
        Color color = skinnedMeshRenderer.material.color;
        skinnedMeshRenderer.material.SetColor("_Color", new Color(color.r, color.g, color.b, alpha));
    }
}
