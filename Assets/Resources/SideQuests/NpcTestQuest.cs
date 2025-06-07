using UnityEngine;

public class NpcTestQuest : SideQuestStep
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Cucoo"))
        {
            Debug.Log("Hey");
            ProgressStep();
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
