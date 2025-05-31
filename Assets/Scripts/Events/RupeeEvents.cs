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

    public event Action<int> onRupeeUsed;
    public void OnRupeeUsed(int value)
    {
        if (onRupeeUsed != null)
        {
            onRupeeUsed(value);
        }
    }
}
