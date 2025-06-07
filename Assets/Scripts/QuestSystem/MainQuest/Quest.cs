using UnityEngine;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state;
    private int currentQuestStepIndex;

    public Quest(QuestInfoSO questInfo)
    {
        this.info = questInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return currentQuestStepIndex < info.questStepPrefabs.Length;
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            if (questStep != null)
            {
                questStep.InitializeQuestStep(info.id, info);
            }
            else
            {
                Object.Destroy(questStepPrefab);
            }
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        if (CurrentStepExists())
        {
            return info.questStepPrefabs[currentQuestStepIndex];
        }
        return null;
    }
}
