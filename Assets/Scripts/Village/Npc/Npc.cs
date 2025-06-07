using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogueData;
    protected bool isPlayerInRange = false;
    protected bool inTalk = false;

    protected virtual void Start()
    {
        GameEventsManager.instance.villageEvents.onTalkStateChange += OnTalkStateChange;
    }

    protected virtual void OnDisable()
    {
        GameEventsManager.instance.villageEvents.onTalkStateChange -= OnTalkStateChange;
    }

    protected virtual void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.O) && !inTalk)
        {
            inTalk = true;
            GameEventsManager.instance.playerEvents.OnActionChange();
            DialogueManager.Instance.StartDialogue(dialogueData, 0);
        }
    }

    protected void OnTalkStateChange()
    {
        inTalk = !inTalk;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
