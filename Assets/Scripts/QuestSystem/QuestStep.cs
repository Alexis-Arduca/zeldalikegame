using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private bool gameIsFinished = false;
    private string questId;
    private string sideQuestId;

    private void Start()
    {}

    public void InitializeQuestStep(string questId)
    {
        this.questId = questId;
    }

    public void InitializeSideQuestStep(string questId)
    {
        this.sideQuestId = questId;
    }

    protected void VerificationQuestStep()
    {
        GameEventsManager.instance.questEvents.ProgressQuest(questId);
    }

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            GameEventsManager.instance.questEvents.FinishQuest(questId);
            Destroy(this.gameObject);
        }
    }

    protected void FinishSideQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            GameEventsManager.instance.questEvents.FinishSideQuest(sideQuestId);
            Destroy(this.gameObject);
        }
    }
}
