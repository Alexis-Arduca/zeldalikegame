using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    private List<Quest> mainQuests = new List<Quest>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadMainQuests(Resources.LoadAll<QuestInfoSO>("MainQuests"));
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onProgressQuest += ProgressQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onProgressQuest -= ProgressQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;
    }

    public void LoadMainQuests(QuestInfoSO[] allMainQuestSOs)
    {
        foreach (var questSO in allMainQuestSOs)
        {
            Quest quest = new Quest(questSO);
            mainQuests.Add(quest);
        }
    }

    private void StartQuest(QuestInfoSO questInfo)
    {
        var quest = mainQuests.Find(q => q.info.id == questInfo.id);
        if (quest != null)
        {
            quest.state = QuestState.CAN_START;
            quest.InstantiateCurrentQuestStep(this.transform);
            quest.state = QuestState.IN_PROGRESS;
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void ProgressQuest(QuestInfoSO questInfo)
    {
        var quest = mainQuests.Find(q => q.info.id == questInfo.id);
        if (quest != null)
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
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void FinishQuest(QuestInfoSO questInfo)
    {
        var quest = mainQuests.Find(q => q.info.id == questInfo.id);
        if (quest != null && quest.state == QuestState.CAN_FINISH)
        {
            quest.state = QuestState.FINISHED;
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    public List<Quest> GetActiveMainQuests()
    {
        var activeQuests = mainQuests.FindAll(q => q.state == QuestState.CAN_START || q.state == QuestState.IN_PROGRESS || q.state == QuestState.CAN_FINISH);

        return activeQuests;
    }

    public List<Quest> GetCompletedQuests()
    {
        var completedQuests = mainQuests.FindAll(q => q.state == QuestState.FINISHED);

        return completedQuests;
    }
}
