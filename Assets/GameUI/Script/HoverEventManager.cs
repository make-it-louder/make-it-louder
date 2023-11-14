using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject componentToShow;

    public void OnPointerEnter(PointerEventData eventData)
    {
        componentToShow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    { 
        componentToShow.SetActive(false);
    }
}
