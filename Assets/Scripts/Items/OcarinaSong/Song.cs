using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Song
{
    public string name;
    public List<KeyCode> sequence;
    public AudioClip songClip;

    public void PlayEffect()
    {
        Debug.Log($"Playing effect of song: {name}");

        // Song Effect here
    }

    public bool DoesSequenceMatch(List<KeyCode> inputSequence)
    {
        if (inputSequence.Count != sequence.Count) return false;

        for (int i = 0; i < sequence.Count; i++)
        {
            if (inputSequence[i] != sequence[i]) return false;
        }

        return true;
    }
}
