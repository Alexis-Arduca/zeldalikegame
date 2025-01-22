using System.Collections.Generic;
using UnityEngine;

public class OcarinaManager : MonoBehaviour
{
    public Ocarina equippedOcarina;
    private List<KeyCode> currentSequence = new List<KeyCode>();
    private float inputTimeout = 2f;
    private float timer;

    void Update()
    {
        if (equippedOcarina == null)
        {
            return;
        }

        if (currentSequence.Count > 0)
        {
            timer += Time.deltaTime;
            Debug.Log($"Timer: {timer}, Timeout: {inputTimeout}");

            if (timer > inputTimeout)
            {
                Debug.Log("Sequence timed out. Clearing input.");
                currentSequence.Clear();
            }
        }

        foreach (KeyCode key in GetValidKeys())
        {
            if (Input.GetKeyDown(key))
            {
                Debug.Log($"Key pressed: {key}");
                currentSequence.Add(key);
                Debug.Log("Current sequence: " + string.Join(", ", currentSequence));
                timer = 0;
                CheckSongMatch();
            }
        }
    }

    private void CheckSongMatch()
    {
        Debug.Log("Checking if sequence matches any song...");
        foreach (var song in equippedOcarina.songs)
        {
            Debug.Log($"Checking song: {song.name} with sequence: {string.Join(", ", song.sequence)}");

            if (song.DoesSequenceMatch(currentSequence))
            {
                Debug.Log($"Song matched: {song.name}");
                song.PlayEffect();
                currentSequence.Clear();
                return;
            }
        }

        Debug.Log("No matching song found.");
    }

    private List<KeyCode> GetValidKeys()
    {
        List<KeyCode> validKeys = new List<KeyCode> 
        { 
            KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, 
            KeyCode.RightArrow, KeyCode.Q, KeyCode.Z 
        };
        return validKeys;
    }
}
