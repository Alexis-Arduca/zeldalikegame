using UnityEngine;

public class SideQuestStep : QuestStep
{
    protected override void OnStepCompleted()
    {
        if (questInfo is SideQuestSO sideQuestSO)
        {
            GameEventsManager.instance.questEvents.ProgressSideQuest(sideQuestSO);
            Destroy(gameObject);
        }
    }

    public override void InitializeQuestStep(string questId, ScriptableObject questInfo)
    {
        base.InitializeQuestStep(questId, questInfo);
    }
}
