using System;
using UnityEngine;

public class VillageEvents
{
    public event Action onTalkStateChange;
    public void OnTalkStateChange()
    {
        if (onTalkStateChange != null)
        {
            onTalkStateChange();
        }
    }
}
