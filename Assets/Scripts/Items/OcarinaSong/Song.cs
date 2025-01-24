using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Song
{
    public string name;
    public int id;
    public List<KeyCode> sequence;
    public AudioClip songClip;

    public void PlayEffect()
    {
        Debug.Log($"Playing effect of song: {name}");

        switch (id)
        {
            case 1:
                Debug.Log("Healing effect activated!");
                break;

            case 2:
                Debug.Log("Time manipulation effect activated!");
                break;

            case 3:
                Debug.Log("Storm effect activated!");
                break;

            case 4:
                Debug.Log("Activating teleportation mode...");
                ShowTeleportMenu();
                break;

            default:
                Debug.Log("No specific effect for this song.");
                break;
        }
    }

    private void ShowTeleportMenu()
    {
        TeleportPoint[] teleportPoints = GameObject.FindObjectsOfType<TeleportPoint>();

        var teleportMenu = GameEventsManager.instance.mapUI;
        if (teleportMenu != null)
        {
            teleportMenu.SetActive(true);
            teleportMenu.GetComponent<MapUI>().PopulateMenu(teleportPoints);

            foreach (var point in teleportPoints)
            {
                Debug.Log($"Point available: {point.pointName}");
            }
        }
        else
        {
            Debug.LogWarning("TeleportMenu not found in the scene.");
        }
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
