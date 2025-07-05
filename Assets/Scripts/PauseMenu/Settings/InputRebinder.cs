using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputActionAsset playerControls;
    public string actionMapName = "Player";
    public string actionName = "Menu";

    private InputActionMap actionMap;
    private InputAction actionToRebind;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    void Start()
    {
        actionMap = playerControls.FindActionMap(actionMapName);
        if (actionMap == null)
        {
            Debug.LogError($"ActionMap '{actionMapName}' non trouvé.");
            return;
        }

        actionToRebind = actionMap.FindAction(actionName);
        if (actionToRebind == null)
        {
            Debug.LogError($"Action '{actionName}' non trouvée.");
            return;
        }

        actionMap.Enable();
    }

    public void StartRebind(int bindingIndex = 0)
    {
        if (actionToRebind == null) { return; }

        Debug.Log($"Démarrage du rebind pour {actionName}, binding index {bindingIndex}");
        actionToRebind.Disable();

        rebindOperation = actionToRebind
            .PerformInteractiveRebinding(bindingIndex)
            .WithControlsHavingToMatchPath("Keyboard")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation =>
            {
                operation.Dispose();
                actionToRebind.Enable();
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                actionToRebind.Enable();
            })
            .OnPotentialMatch(operation =>
            {
                Debug.Log($"Touch Input : {operation.selectedControl.path}");
            })
            .Start();
    }
}
