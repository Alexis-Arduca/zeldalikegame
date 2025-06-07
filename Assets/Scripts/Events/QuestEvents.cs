using System;

public class QuestEvents
{
    public event Action<QuestInfoSO> onStartQuest;
    public void StartQuest(QuestInfoSO questInfo)
    {
        UnityEngine.Debug.Log($"Événement StartQuest déclenché pour la quête : {questInfo.id}");
        onStartQuest?.Invoke(questInfo);
    }

    public event Action<QuestInfoSO> onProgressQuest;
    public void ProgressQuest(QuestInfoSO questInfo)
    {
        UnityEngine.Debug.Log($"Événement ProgressQuest déclenché pour la quête : {questInfo.id}");
        onProgressQuest?.Invoke(questInfo);
    }

    public event Action<QuestInfoSO> onFinishQuest;
    public void FinishQuest(QuestInfoSO questInfo)
    {
        UnityEngine.Debug.Log($"Événement FinishQuest déclenché pour la quête : {questInfo.id}");
        onFinishQuest?.Invoke(questInfo);
    }

    public event Action<Quest> onQuestStateChange;
    public void QuestStateChange(Quest quest)
    {
        UnityEngine.Debug.Log($"Événement QuestStateChange déclenché pour la quête : {quest.info.id}");
        onQuestStateChange?.Invoke(quest);
    }

    public event Action<SideQuestSO> onStartSideQuest;
    public void StartSideQuest(SideQuestSO questInfo)
    {
        UnityEngine.Debug.Log($"Événement StartSideQuest déclenché pour la quête : {questInfo.id}");
        onStartSideQuest?.Invoke(questInfo);
    }

    public event Action<SideQuestSO> onProgressSideQuest;
    public void ProgressSideQuest(SideQuestSO questInfo)
    {
        UnityEngine.Debug.Log($"Événement ProgressSideQuest déclenché pour la quête : {questInfo.id}");
        onProgressSideQuest?.Invoke(questInfo);
    }

    public event Action<SideQuestSO> onFinishSideQuest;
    public void FinishSideQuest(SideQuestSO questInfo)
    {
        UnityEngine.Debug.Log($"Événement FinishSideQuest déclenché pour la quête : {questInfo.id}");
        onFinishSideQuest?.Invoke(questInfo);
    }

    public event Action<SideQuest> onSideQuestStateChange;
    public void SideQuestStateChange(SideQuest quest)
    {
        UnityEngine.Debug.Log($"Événement SideQuestStateChange déclenché pour la quête : {quest.info.id}");
        onSideQuestStateChange?.Invoke(quest);
    }
}