using UnityEngine;

public class MainQuestStep : QuestStep
{
    protected override void OnStepCompleted()
    {
        if (questInfo is QuestInfoSO questInfoSO)
        {
            GameEventsManager.instance.questEvents.ProgressQuest(questInfoSO);
            Destroy(gameObject);
        }
    }

    public override void InitializeQuestStep(string questId, ScriptableObject questInfo)
    {
        base.InitializeQuestStep(questId, questInfo);
    }
}
