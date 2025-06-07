using System.Collections.Generic;
using UnityEngine;

public class SideQuestManager : MonoBehaviour
{
    public static SideQuestManager Instance { get; private set; }

    private Dictionary<string, SideQuest> sideQuestMap = new Dictionary<string, SideQuest>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        LoadAllSideQuests();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onStartSideQuest += StartQuest;
        GameEventsManager.instance.questEvents.onProgressSideQuest += ProgressQuest;
        GameEventsManager.instance.questEvents.onFinishSideQuest += FinishQuest;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onStartSideQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onProgressSideQuest -= ProgressQuest;
        GameEventsManager.instance.questEvents.onFinishSideQuest -= FinishQuest;
    }

    public void LoadAllSideQuests()
    {
        var allSideQuests = Resources.LoadAll<SideQuestSO>("SideQuests");

        foreach (var questSO in allSideQuests)
        {
            if (!sideQuestMap.ContainsKey(questSO.id))
            {
                SideQuest quest = new SideQuest(questSO);
                sideQuestMap.Add(questSO.id, quest);
            }
        }
    }

    private void StartQuest(SideQuestSO questInfo)
    {
        Debug.Log($"StartQuest called with the quest ID : {questInfo.id} (Name : {questInfo.displayName})");

        if (sideQuestMap.TryGetValue(questInfo.id, out SideQuest quest))
        {
            quest.state = QuestState.CAN_START;

            quest.InstantiateCurrentQuestStep(this.transform);

            quest.state = QuestState.IN_PROGRESS;

            GameEventsManager.instance.questEvents.SideQuestStateChange(quest);
        }
        else
        {
            Debug.LogWarning($"Failure : Quest with ID {questInfo.id} not found in sideQuestMap.");
        }
    }

    private void ProgressQuest(SideQuestSO questInfo)
    {
        if (sideQuestMap.TryGetValue(questInfo.id, out SideQuest quest))
        {
            quest.MoveToNextStep();
            if (quest.CurrentStepExists())
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            else
            {
                quest.state = QuestState.CAN_FINISH;
            }
            GameEventsManager.instance.questEvents.SideQuestStateChange(quest);
        }
    }

    private void FinishQuest(SideQuestSO questInfo)
    {
        if (sideQuestMap.TryGetValue(questInfo.id, out SideQuest quest))
        {
            if (quest.state == QuestState.CAN_FINISH)
            {
                quest.state = QuestState.FINISHED;
                GameEventsManager.instance.questEvents.SideQuestStateChange(quest);
                sideQuestMap.Remove(questInfo.id);
            }
        }
    }

    public List<SideQuest> GetActiveSideQuests()
    {
        List<SideQuest> activeQuests = new List<SideQuest>();
        foreach (var quest in sideQuestMap.Values)
        {
            if (quest.state == QuestState.CAN_START || 
                quest.state == QuestState.IN_PROGRESS || 
                quest.state == QuestState.CAN_FINISH)
            {
                activeQuests.Add(quest);
                Debug.Log($"Quête active trouvée : {quest.info.id}, État : {quest.state}");
            }
        }
        Debug.Log($"Nombre de quêtes actives retournées : {activeQuests.Count}");
        return activeQuests;
    }

    public List<SideQuest> GetCompletedSideQuests()
    {
        List<SideQuest> completedQuests = new List<SideQuest>();
        foreach (var quest in sideQuestMap.Values)
        {
            if (quest.state == QuestState.FINISHED)
            {
                completedQuests.Add(quest);
                Debug.Log($"Quête terminée trouvée : {quest.info.id}");
            }
        }
        Debug.Log($"Nombre de quêtes secondaires terminées retournées : {completedQuests.Count}");
        return completedQuests;
    }

    public SideQuest GetSideQuestById(string id)
    {
        sideQuestMap.TryGetValue(id, out SideQuest quest);
        return quest;
    }

    public void AddSideQuest(SideQuestSO questSO)
    {
        if (!sideQuestMap.ContainsKey(questSO.id))
        {
            var newQuest = new SideQuest(questSO);
            sideQuestMap.Add(questSO.id, newQuest);
            newQuest.state = QuestState.CAN_START;
            newQuest.InstantiateCurrentQuestStep(this.transform);
            newQuest.state = QuestState.IN_PROGRESS;
            GameEventsManager.instance.questEvents.SideQuestStateChange(newQuest);
            GameEventsManager.instance.questEvents.StartSideQuest(questSO);
        }
    }
}
