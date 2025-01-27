using System;
using UnityEngine;

public class RupeeEvents
{
    public event Action<int> onRupeeCollected;
    public void OnRupeeCollected(int value)
    {
        if (onRupeeCollected != null)
        {
            onRupeeCollected(value);
        }
    }
}
