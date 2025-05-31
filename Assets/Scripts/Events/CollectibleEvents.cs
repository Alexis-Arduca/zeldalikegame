using System;
using UnityEngine;

public class CollectibleEvents
{
    public event Action<int> onBombCollected;
    public void OnBombCollected(int value)
    {
        if (onBombCollected != null)
        {
            onBombCollected(value);
        }
    }

    public event Action<int> onArrowCollected;
    public void OnArrowCollected(int value)
    {
        if (onArrowCollected != null)
        {
            onArrowCollected(value);
        }
    }

    public event Action<double> onRecoveryHeartCollected;
    public void OnRecoveryHeartCollected(double value)
    {
        if (onRecoveryHeartCollected != null)
        {
            onRecoveryHeartCollected(value);
        }
    }
}
