using UnityEngine;

public class ButtonGroupManager : MonoBehaviour
{
    private ButtonData currentSelected;

    public void SetSelected(ButtonData button)
    {
        if (currentSelected != null && currentSelected != button)
        {
            currentSelected.DeselectExternally();
        }

        currentSelected = button;
    }
}
