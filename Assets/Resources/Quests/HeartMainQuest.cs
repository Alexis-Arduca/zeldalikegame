using UnityEngine;

public class HeartMainQuest : MainQuestStep
{
    private int heartCollected = 0;
    private int heartToComplete = 4;

    private void Start()
    {
        GameEventsManager.instance.miscEvents.onHeartContainerCollected += HeartCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onHeartContainerCollected -= HeartCollected;
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
        Debug.Log($"Initialisation de l'étape de quête principale HeartMainQuest : {questId}");
    }

    protected override void OnStepCompleted()
    {
        base.OnStepCompleted();
    }
}
