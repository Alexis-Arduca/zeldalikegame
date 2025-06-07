using UnityEngine;

public class HeartFragmentSideQuest : SideQuestStep
{
    private int heartCollected = 0;
    private int heartToComplete = 4;

    private void Start()
    {
        GameEventsManager.instance.miscEvents.onHeartFragmentCollected += HeartCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onHeartFragmentCollected -= HeartCollected;
    }

    private void HeartCollected()
    {
        if (heartCollected < heartToComplete)
        {
            heartCollected++;
        }
        if (heartCollected >= heartToComplete)
        {
            ProgressStep();
        }
    }

    public override void InitializeQuestStep(string questId, ScriptableObject questInfo)
    {
        base.InitializeQuestStep(questId, questInfo);
        Debug.Log($"Initialisation de l'étape de quête secondaire HeartFragmentSideQuest : {questId}");
    }

    protected override void OnStepCompleted()
    {
        base.OnStepCompleted();
    }
}