using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideQuestAssignement : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private SideQuestManager sideQuestManager;
    [SerializeField] private SideQuestSO questInfoForPoint;
    
    [Header("Interaction Settings")]
    [SerializeField] private KeyCode interactionKey = KeyCode.O;

    private bool playerIsNear = false;
    private bool isQuestAssigned = false;
    private string questId;
    private QuestState currentQuestState;

    private void Awake() 
    {
        questId = questInfoForPoint.id;
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
        }
    }

    private void Update()
    {
        if (playerIsNear && !isQuestAssigned && Input.GetKeyDown(interactionKey))
        {
            AssignQuest();
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player") && !isQuestAssigned)
        {
            playerIsNear = true;
            Debug.Log("Enter");
        }
    }

    void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }

    private void AssignQuest()
    {
        isQuestAssigned = true;
        sideQuestManager.AddSideQuest(questInfoForPoint);
        Debug.Log("Quête assignée : " + questInfoForPoint.name);
    }
}
