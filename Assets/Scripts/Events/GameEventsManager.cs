using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }
    public GameObject mapUI;

    public QuestEvents questEvents;
    public RupeeEvents rupeeEvents;
    public MagicEvents magicEvents;
    public MiscEvents miscEvents;
    public PlayerEvents playerEvents;
    public CollectibleEvents collectibleEvents;
    public VillageEvents villageEvents;
    public CocoricoQuestEvents cocoricoQuestEvents;
    public PauseEvents pauseEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;

        //         // initialize all events
        //         inputEvents = new InputEvents();
        questEvents = new QuestEvents();
        rupeeEvents = new RupeeEvents();
        magicEvents = new MagicEvents();
        miscEvents = new MiscEvents();
        playerEvents = new PlayerEvents();
        collectibleEvents = new CollectibleEvents();
        villageEvents = new VillageEvents();
        cocoricoQuestEvents = new CocoricoQuestEvents();
        pauseEvents = new PauseEvents();
    }
}