using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public GameObject choicesPanel;
    public Button choiceButtonPrefab;

    private Dialogue currentDialogue;
    private int currentIndex;
    private bool isTyping;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        choicesPanel.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue, int startIndex = 0)
    {
        if (dialogue == null || dialogue.lines == null || dialogue.lines.Count == 0) return;

        currentDialogue = dialogue;
        currentIndex = startIndex;
        dialoguePanel.SetActive(true);
        nameText.text = dialogue.npcName;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        DialogueLine line = currentDialogue.lines[currentIndex];
        string displayText = RemoveSpecialTags(line.text);
        foreach (char c in displayText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(currentDialogue.typingSpeed);
        }

        isTyping = false;
        HandleTags(line.tags);

        if (line.choices != null && line.choices.Count > 0)
        {
            ShowChoices(line.choices);
        }
        else if (line.isEndOfBranch)
        {
            yield return new WaitForSeconds(1.5f);
            EndDialogue();
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            NextLine();
        }
    }

    private void HandleTags(string tags)
    {
        if (string.IsNullOrEmpty(tags)) return;

        string[] tagArray = tags.Split(' ');
        foreach (string tag in tagArray)
        {
            if (tag.StartsWith("#ACCEPT_SIDEQUEST:"))
            {
                string questId = tag.Replace("#ACCEPT_SIDEQUEST:", "");
                SideQuestSO quest = Resources.Load<SideQuestSO>($"SideQuests/{questId}");
                if (quest != null) GameEventsManager.instance.questEvents.StartSideQuest(quest);
            }
            else if (tag.StartsWith("#COMPLETE_SIDEQUEST:"))
            {
                string questId = tag.Replace("#COMPLETE_SIDEQUEST:", "");
                SideQuestSO quest = Resources.Load<SideQuestSO>($"SideQuests/{questId}");
                GameEventsManager.instance.cocoricoQuestEvents.OnCucooQuestComplete();
                GameEventsManager.instance.questEvents.FinishSideQuest(quest);
            }
                else if (tag.StartsWith("#ACCEPT_MAINQUEST:"))
                {
                    string questId = tag.Replace("#ACCEPT_MAINQUEST:", "");
                    QuestInfoSO quest = Resources.Load<QuestInfoSO>($"MainQuests/{questId}");
                    if (quest != null) GameEventsManager.instance.questEvents.StartQuest(quest);
                }
                else if (tag.StartsWith("#COMPLETE_MAINQUEST:"))
                {
                    string questId = tag.Replace("#COMPLETE_MAINQUEST:", "");
                    QuestInfoSO quest = Resources.Load<QuestInfoSO>($"MainQuests/{questId}");
                    if (quest != null) GameEventsManager.instance.questEvents.FinishQuest(quest);
                }
        }
    }

    private string RemoveSpecialTags(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        string result = text;
        int startIndex = result.IndexOf("#");
        while (startIndex != -1)
        {
            int endIndex = result.IndexOf(" ", startIndex);
            if (endIndex == -1) endIndex = result.Length;
            result = result.Remove(startIndex, endIndex - startIndex);
            startIndex = result.IndexOf("#");
        }
        return result.Trim();
    }

    private void ShowChoices(List<DialogueChoice> choices)
    {
        choicesPanel.SetActive(true);

        foreach (Transform child in choicesPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (DialogueChoice choice in choices)
        {
            Button button = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            button.GetComponentInChildren<TMP_Text>().text = choice.text;
            button.onClick.AddListener(() => OnChoiceSelected(choice.nextLineIndex));
        }
    }

    public void OnChoiceSelected(int nextIndex)
    {
        choicesPanel.SetActive(false);
        if (nextIndex >= 0 && nextIndex < currentDialogue.lines.Count)
        {
            currentIndex = nextIndex;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    public void NextLine()
    {
        if (isTyping) return;

        if (currentIndex + 1 < currentDialogue.lines.Count)
        {
            currentIndex++;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        GameEventsManager.instance.playerEvents.OnActionChange();
        GameEventsManager.instance.villageEvents.OnTalkStateChange();
        dialoguePanel.SetActive(false);
        currentDialogue = null;
        currentIndex = 0;
    }

    private void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (!isTyping && (currentDialogue.lines[currentIndex].choices == null || currentDialogue.lines[currentIndex].choices.Count == 0))
            {
                NextLine();
            }
        }
    }
}
