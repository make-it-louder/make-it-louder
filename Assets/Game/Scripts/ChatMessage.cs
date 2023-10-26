using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class ChatMessage : MonoBehaviour
{
    private TextMeshProUGUI tmpText;
    private RectTransform rectTransform;

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText()
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, tmpText.preferredHeight);
    }
}
