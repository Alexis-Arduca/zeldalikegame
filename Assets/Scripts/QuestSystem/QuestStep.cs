using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    protected string questId;
    protected ScriptableObject questInfo;
    protected bool isFinished = false;

    public virtual void InitializeQuestStep(string questId, ScriptableObject questInfo)
    {
        this.questId = questId;
        this.questInfo = questInfo;
    }

    public void ProgressStep()
    {
        if (isFinished) return;
        isFinished = true;
        OnStepCompleted();
    }

    protected abstract void OnStepCompleted();
}
