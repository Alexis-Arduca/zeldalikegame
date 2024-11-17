using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap = new Dictionary<string, Quest>();
    private Quest currentQuest;
    private int questIndex = 0;
    private QuestState currentQuestState;
    [SerializeField] private SideQuestManager sideQuestManager;

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onProgressQuest -= ProgressQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;
    }

    private void Awake()
    {
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onProgressQuest += ProgressQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;

        questMap = CreateQuestMap("Quests");
    }

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void Update()
    {
        List<Quest> questList = questMap.Values.ToList();

        if (questList.Count > 0)
        {
            Quest firstQuest = questList[questIndex];
            currentQuest = GetQuestById(firstQuest.info.id);
        }
        currentQuestState = currentQuest.state;

        if (currentQuestState.Equals(QuestState.REQUIREMENTS_NOT_MET))
        {
            GameEventsManager.instance.questEvents.StartQuest(currentQuest.info.id);
        }
        else if (currentQuestState.Equals(QuestState.FINISHED))
        {
            questIndex++;
        }
    }

    ////////// Quest Update //////////

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        if (quest == null) return;
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
        quest.InstantiateCurrentQuestStep(this.transform);
    }

    private void ProgressQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ChangeQuestState(quest.info.id, QuestState.AFTER_PROGRESS);
        quest.InstantiateCurrentQuestStep(this.transform);
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);

        quest.MoveToNextStep();
        if (quest.CurrentStepExists())
        {
            ChangeQuestState(quest.info.id, QuestState.REQUIREMENTS_NOT_MET);
            return;
        }

        ChangeQuestState(quest.info.id, QuestState.FINISHED);

        // Load all side quests when the first main quest is finished
        // Uncomment if you want to load all side quests at this point
        // sideQuestManager.LoadAllSideQuests();
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        if (quest != null)
        {
            quest.state = state;
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    ////////// Quest Dictionary //////////
    private Dictionary<string, Quest> CreateQuestMap(string folder)
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>(folder);
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        Debug.Log($"Total quests loaded: {allQuests.Length}");

        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning($"Duplicate ID detected: {questInfo.id}");
            }
            else
            {
                Quest quest = new Quest(questInfo);
                idToQuestMap.Add(questInfo.id, quest);
                Debug.Log($"Added quest with ID: {questInfo.id}");
            }
        }

        Debug.Log($"Total quests in dictionary: {idToQuestMap.Count}");
        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        if (questMap.TryGetValue(id, out Quest quest))
        {
            return quest;
        }
        else
        {
            Debug.LogError("Quest ID not found in the Quest Map: " + id);
            return null;
        }
    }

    public void SetQuest(string id)
    {
        if (questMap.TryGetValue(id, out Quest quest))
        {
            currentQuest = quest;
        }
        else
        {
            Debug.LogError("Quest ID not found: " + id);
        }
    }

    public List<Quest> GetActiveMainQuests()
    {
        return questMap.Values
            .Where(quest => quest.state == QuestState.IN_PROGRESS || quest.state == QuestState.REQUIREMENTS_NOT_MET)
            .ToList();
    }

    public List<Quest> GetCompletedQuests()
    {
        return questMap.Values
            .Where(quest => quest.state == QuestState.FINISHED)
            .ToList();
    }
}
