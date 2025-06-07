using UnityEngine;

public class MoreQuestLater : MainQuestStep
{
    private void Start()
    {
        // Logique à ajouter plus tard
    }

    private void Update()
    {
        // Logique à ajouter plus tard
    }

    public override void InitializeQuestStep(string questId, ScriptableObject questInfo)
    {
        base.InitializeQuestStep(questId, questInfo);
        Debug.Log($"Initialisation de l'étape de quête principale MoreQuestLater : {questId}");
    }

    protected override void OnStepCompleted()
    {
        base.OnStepCompleted();
    }
}
