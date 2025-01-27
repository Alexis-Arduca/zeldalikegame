using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }
    public GameObject mapUI;

//     public InputEvents inputEvents;
//     public PlayerEvents playerEvents;
    public QuestEvents questEvents;
    public RupeeEvents rupeeEvents;
    public MiscEvents miscEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;

//         // initialize all events
//         inputEvents = new InputEvents();
//         playerEvents = new PlayerEvents();
        questEvents = new QuestEvents();
        rupeeEvents = new RupeeEvents();
        miscEvents = new MiscEvents();
    }
}