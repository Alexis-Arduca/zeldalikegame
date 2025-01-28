using System;
using UnityEngine;

public class MagicEvents
{
    public event Action<int> onMagicCollected;
    public void OnMagicCollected(int value)
    {
        if (onMagicCollected != null)
        {
            onMagicCollected(value);
        }
    }
}
