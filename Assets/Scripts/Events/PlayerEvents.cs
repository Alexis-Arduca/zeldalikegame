using System;
using UnityEngine;

public class PlayerEvents
{
    public event Action onActionState;
    public void OnActionChange()
    {
        if (onActionState != null)
        {
            onActionState();
        }
    }
}
