using System;
using UnityEngine;

public class CocoricoQuestEvents
{
    public event Action onCucooDetect;
    public void OnCucooDetect()
    {
        if (onCucooDetect != null)
        {
            onCucooDetect();
        }
    }

    public event Action onCucooQuestComplete;
    public void OnCucooQuestComplete()
    {
        if (onCucooQuestComplete != null)
        {
            onCucooQuestComplete();
        }
    }
}
