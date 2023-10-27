using UnityEngine;
using UnityEngine.EventSystems;

public class RangeSliderControl : MonoBehaviour, IDragHandler
{
    public RectTransform minHandle;
    public RectTransform maxHandle;
    public RectTransform sliderBackground;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(sliderBackground, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            float normalizedValue = Mathf.InverseLerp(sliderBackground.rect.min.x, sliderBackground.rect.max.x, localPoint.x);
            float handleWidth = minHandle.rect.width / 2.0f;

            // Min 핸들의 위치 업데이트
            if (eventData.pointerDrag == minHandle.gameObject)
            {
                float minPosition = Mathf.Clamp(normalizedValue, 0, maxHandle.anchorMin.x - handleWidth);
                minHandle.anchorMin = new Vector2(minPosition, minHandle.anchorMin.y);
                minHandle.anchorMax = new Vector2(minPosition, minHandle.anchorMax.y);
            }
            // Max 핸들의 위치 업데이트
            else if (eventData.pointerDrag == maxHandle.gameObject)
            {
                float maxPosition = Mathf.Clamp(normalizedValue, minHandle.anchorMax.x + handleWidth, 1);
                maxHandle.anchorMin = new Vector2(maxPosition, maxHandle.anchorMin.y);
                maxHandle.anchorMax = new Vector2(maxPosition, maxHandle.anchorMax.y);
            }
        }
    }
}
