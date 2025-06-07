using UnityEngine;

public class NPC_Quest : NPC
{
    [Header("Quest Dialogues")]
    public SideQuestSO sideQuestToGive;
    public Dialogue dialogueQuestInProgress;
    public Dialogue dialogueQuestDone;
    public Dialogue dialogueComplete;

    [Header("Quest Reward")]
    public GameObject reward;

    private bool questGiven = false;
    private bool questCompleted = false;

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onSideQuestStateChange += OnSideQuestStateChange;
    }

    protected override void OnDisable()
    {
        GameEventsManager.instance.questEvents.onSideQuestStateChange -= OnSideQuestStateChange;
    }

    private void OnSideQuestStateChange(SideQuest quest)
    {
        if (quest.info.id == sideQuestToGive.id)
        {
            if (quest.state == QuestState.IN_PROGRESS || quest.state == QuestState.CAN_FINISH)
            {
                questGiven = true;
            }
            else if (quest.state == QuestState.FINISHED)
            {
                questGiven = true;
                questCompleted = true;
            }
        }
    }

    protected override void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.O) && !inTalk)
        {
            inTalk = true;

            GameEventsManager.instance.playerEvents.OnActionChange();

            if (!questGiven)
            {
                DialogueManager.Instance.StartDialogue(dialogueData);
                questGiven = true;
            }
            else
            {
                var quest = SideQuestManager.Instance.GetSideQuestById(sideQuestToGive.id);

                if (questCompleted)
                {
                    DialogueManager.Instance.StartDialogue(dialogueComplete);
                }
                else if (quest != null && quest.state == QuestState.IN_PROGRESS)
                {
                    DialogueManager.Instance.StartDialogue(dialogueQuestInProgress);
                }
                else if (quest != null && quest.state == QuestState.CAN_FINISH)
                {
                    DialogueManager.Instance.StartDialogue(dialogueQuestDone);
                    GameEventsManager.instance.questEvents.FinishSideQuest(sideQuestToGive);
                    PlayerReward();
                    questCompleted = true;
                }
                else
                {
                    DialogueManager.Instance.StartDialogue(dialogueQuestInProgress);
                }
            }
        }
    }

    private void PlayerReward()
    {
        if (reward != null)
        {
            var collectible = reward.GetComponent<Collectibles>();
            if (collectible != null)
            {
                collectible.OnBuy();
            }
        }
    }
}
