using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonData : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject affiliateMenu;
    public List<GameObject> highlightedObjects;
    private bool isSelected;

    void Start()
    {
        if (affiliateMenu != null) { affiliateMenu.SetActive(false); }
        foreach (GameObject o in highlightedObjects) { o.SetActive(false); }
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;

        if (affiliateMenu != null) { affiliateMenu.SetActive(true); }

        HighlightsObjects(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;

        if (affiliateMenu != null) { affiliateMenu.SetActive(false); }

        HighlightsObjects(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HighlightsObjects(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HighlightsObjects(false);
    }

    private void HighlightsObjects(bool state)
    {
        if (state == false && isSelected == true) { return; }

        foreach (GameObject obj in highlightedObjects) { obj.SetActive(state); }
    }
}
