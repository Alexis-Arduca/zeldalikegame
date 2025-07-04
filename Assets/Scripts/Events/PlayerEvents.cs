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

    public event Action onPlayerSpawn;
    public void OnPlayerSpawn()
    {
        if (onPlayerSpawn != null)
        {
            onPlayerSpawn();
        }
    }

    public event Action onPlayerOpenMenu;
    public void OnPlayerOpenMenu()
    {
        if (onPlayerOpenMenu != null)
        {
            onPlayerOpenMenu();
        }
    }

    public event Action onPlayerInteract;
    public void OnPlayerInteract()
    {
        if (onPlayerInteract != null)
        {
            onPlayerInteract();
        }
    }
}
