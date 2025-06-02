using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogueData;
    protected bool isPlayerInRange = false;

    protected virtual void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.O))
        {
            DialogueManager.Instance.StartDialogue(dialogueData, 0);
        }
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
