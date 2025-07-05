using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonData : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    public GameObject affiliateMenu;
    public List<GameObject> highlightedObjects;
    private ButtonGroupManager groupManager;

    void Start()
    {
        groupManager = GetComponentInParent<ButtonGroupManager>();

        if (affiliateMenu != null) { affiliateMenu.SetActive(false); }
        foreach (GameObject o in highlightedObjects) { o.SetActive(false); }
    }

    public void OnSelect(BaseEventData eventData)
    {
        groupManager?.SetSelected(this);
        ShowHighlight(true);
    }

    public void DeselectExternally()
    {
        ShowHighlight(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    private void ShowHighlight(bool state)
    {
        if (affiliateMenu != null) { affiliateMenu.SetActive(state); }
        foreach (GameObject o in highlightedObjects) { o.SetActive(state); }
    }
}
