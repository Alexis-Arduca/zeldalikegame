using System;
using UnityEngine;

public class PauseEvents
{
    public event Action onBindChange;
    public void OnBindChange()
    {
        if (onBindChange != null)
        {
            onBindChange();
        }
    }
}
