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

    private string[] lines;
    private bool[] autoProgress;
    private float typingSpeed;
    private float autoDelay;
    private List<DialogueLineChoices> choices;
    private int index;
    private bool isTyping;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        choicesPanel.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue, int startIndex = 0)
    {
        GameEventsManager.instance.playerEvents.OnActionChange();

        dialoguePanel.SetActive(true);
        nameText.text = dialogue.npcName;
        lines = dialogue.dialogueLines;
        autoProgress = dialogue.autoProgressLines;
        typingSpeed = dialogue.typingSpeed;
        autoDelay = dialogue.autoProgressDelay;
        choices = dialogue.choices;
        index = startIndex;
        StartCoroutine(TypeLine());
    }


    private IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in lines[index])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        if (choices != null && index < choices.Count && choices[index] != null && choices[index].choicesForLine.Count > 0)
        {
            ShowChoices(choices[index].choicesForLine);
        }
        else if (index < autoProgress.Length && autoProgress[index])
        {
            yield return new WaitForSeconds(autoDelay);
            NextLine();
        }
    }

    private void ShowChoices(List<DialogueChoice> currentChoices)
    {
        choicesPanel.SetActive(true);

        foreach (Transform child in choicesPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (DialogueChoice choice in currentChoices)
        {
            Button button = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            button.GetComponentInChildren<TMP_Text>().text = choice.choiceText;
            button.onClick.AddListener(() => OnChoiceSelected(choice.nextLineIndex));
        }
    }

    private void OnChoiceSelected(int nextIndex)
    {
        choicesPanel.SetActive(false);
        index = nextIndex;
        StartCoroutine(TypeLine());
    }

    public void NextLine()
    {
        if (isTyping) return;

        index++;
        if (index < lines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            GameEventsManager.instance.playerEvents.OnActionChange();
            dialoguePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            bool hasChoices = choices != null && index < choices.Count && choices[index] != null && choices[index].choicesForLine.Count > 0;

            if (!isTyping && !hasChoices)
            {
                NextLine();
            }
        }
    }
}
