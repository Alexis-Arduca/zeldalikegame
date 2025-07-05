using UnityEngine;
using UnityEngine.UI;

public class PauseObject : MonoBehaviour
{
    public GameObject menu;
    private bool canOpenInv = true;
    private bool isActive = false;

    void Start()
    {
        GameEventsManager.instance.playerEvents.onActionState += OnActionChange;
        GameEventsManager.instance.playerEvents.onPlayerOpenMenu += OpenMenu;
    }

    void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onActionState -= OnActionChange;
        GameEventsManager.instance.playerEvents.onPlayerOpenMenu -= OpenMenu;
    }

    private void OpenMenu()
    {
        if (canOpenInv) { isActive = !isActive; menu.SetActive(isActive); }
    }

    private void OnActionChange()
    {
        canOpenInv = !canOpenInv;
    }
}
