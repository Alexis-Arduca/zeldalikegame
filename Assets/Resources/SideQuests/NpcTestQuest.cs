using UnityEngine;

public class NpcTestQuest : SideQuestStep
{
    private void Start()
    {
        GameEventsManager.instance.cocoricoQuestEvents.onCucooDetect += CucooDetect;
        GameEventsManager.instance.cocoricoQuestEvents.onCucooQuestComplete += ProgressStep;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.cocoricoQuestEvents.onCucooDetect -= CucooDetect;
        GameEventsManager.instance.cocoricoQuestEvents.onCucooQuestComplete -= ProgressStep;
    }

    private void CucooDetect()
    {
        Debug.Log("Test");

        SideQuestSO sideQuestSO = questInfo as SideQuestSO;
        if (sideQuestSO == null)
        {
            Debug.LogError("questInfo is not a SideQuestSO");
            return;
        }

        SideQuest quest = SideQuestManager.Instance.GetSideQuestById(questId);
        if (quest != null)
        {
            quest.state = QuestState.CAN_FINISH;
        }
    }

    public override void InitializeQuestStep(string questId, ScriptableObject questInfo)
    {
        base.InitializeQuestStep(questId, questInfo);
        Debug.Log($"Initialisation de l'étape de quête secondaire NpcTestQuest : {questId}");
    }

    protected override void OnStepCompleted()
    {
        base.OnStepCompleted();
    }
}
