using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class BindingDisplay : MonoBehaviour
{
    public InputActionReference actionReference;
    public int bindingIndex = 0;
    private string displayString;
    public TMP_Text textRef;
    public string type;

    void Start()
    {
        displayString = GetGamepadBindingDisplay();
        SetTextDisplay();

        GameEventsManager.instance.pauseEvents.onBindChange += SetTextDisplay;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.pauseEvents.onBindChange -= SetTextDisplay;
    }

    public string GetGamepadBindingDisplay()
    {
        if (actionReference == null || actionReference.action == null) { return "N/A"; }

        var action = actionReference.action;
        
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];

            if (binding.isPartOfComposite) { continue; }

            if (binding.groups != null && binding.groups.Contains(type))
            {
                return action.GetBindingDisplayString(i);
            }
        }

        return "N/A";
    }


    public void SetTextDisplay()
    {
        Debug.Log("Bite2");
        textRef.text = GetGamepadBindingDisplay();
    }
}
