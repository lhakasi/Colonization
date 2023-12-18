using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseOverTextRenderer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject objectToRender;

    private void Start()
    {
        if (objectToRender != null)
        {
            objectToRender.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (objectToRender != null)
        {
            objectToRender.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (objectToRender != null)
        {
            objectToRender.SetActive(false);
        }
    }
}