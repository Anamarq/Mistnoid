using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject panel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}
