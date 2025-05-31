using System;
using UnityEngine;

public class MiscEvents
{
    public event Action onHeartContainerCollected;
    public void HeartContainerCollected()
    {
        if (onHeartContainerCollected != null)
        {
            onHeartContainerCollected();
        }
    }

    public event Action onHeartFragmentCollected;
    public void HeartFragmentCollected()
    {
        if (onHeartFragmentCollected != null)
        {
            onHeartFragmentCollected();
        }
    }
}
