using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestMenu : MonoBehaviour
{
    [SerializeField] public GameObject questMenuPanel;
    [SerializeField] private Button mainQuestTab;
    [SerializeField] private Button sideQuestTab;
    [SerializeField] private Button completedQuestTab;
    [SerializeField] private TextMeshProUGUI questListText;

    private bool isMenuVisible = false;
    private bool showingMainQuests = true;
    private bool showingCompletedQuests = false;
    private bool canOpenQuest = true;

    public QuestManager questManager;
    public SideQuestManager sideQuestManager;

    private void Start()
    {
        questMenuPanel.SetActive(false);

        mainQuestTab.onClick.AddListener(ShowMainQuests);
        sideQuestTab.onClick.AddListener(ShowSideQuests);
        completedQuestTab.onClick.AddListener(ShowCompletedQuests);

        UpdateQuestDisplay();

        GameEventsManager.instance.playerEvents.onActionState += OnActionChange;
    }

    void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onActionState -= OnActionChange;
    }

    private void OnActionChange()
    {
        canOpenQuest = !canOpenQuest;
        if (!canOpenQuest)
        {
            questMenuPanel.SetActive(canOpenQuest);
            isMenuVisible = canOpenQuest;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && canOpenQuest)
        {
            ToggleQuestMenu();
        }
    }

    public void ToggleQuestMenu()
    {
        isMenuVisible = !isMenuVisible;
        questMenuPanel.SetActive(isMenuVisible);

        if (isMenuVisible)
        {
            UpdateQuestDisplay();
        }
    }

    private void UpdateQuestDisplay()
    {
        questListText.text = "";

        if (showingCompletedQuests)
        {
            DisplayCompletedQuests();
        }
        else if (showingMainQuests)
        {
            DisplayMainQuests();
        }
        else
        {
            DisplaySideQuests();
        }
    }

    private void ShowMainQuests()
    {
        showingMainQuests = true;
        showingCompletedQuests = false;
        UpdateQuestDisplay();
    }

    private void ShowSideQuests()
    {
        showingMainQuests = false;
        showingCompletedQuests = false;
        UpdateQuestDisplay();
    }

    private void ShowCompletedQuests()
    {
        showingMainQuests = false;
        showingCompletedQuests = true;
        UpdateQuestDisplay();
    }

    private void DisplayMainQuests()
    {
        var mainQuests = questManager.GetActiveMainQuests();
        if (mainQuests.Count == 0)
        {
            questListText.text = "There is no active main quest(s).";
        }
        else
        {
            foreach (var quest in mainQuests)
            {
                questListText.text += $"<b>{quest.info.displayName}</b>:\n{quest.info.Description}\n\n";
            }
        }
    }

    private void DisplaySideQuests()
    {
        var sideQuests = sideQuestManager.GetActiveSideQuests();
        if (sideQuests.Count == 0)
        {
            questListText.text = "There is no active side quest(s).";
        }
        else
        {
            foreach (var quest in sideQuests)
            {
                questListText.text += $"<b>{quest.info.displayName}</b>:\n{quest.info.Description}\n\n";
            }
        }
    }

    private void DisplayCompletedQuests()
    {
        var completedMainQuests = questManager.GetCompletedQuests();
        var completedSideQuests = sideQuestManager.GetCompletedSideQuests();
        
        if (completedMainQuests.Count == 0 && completedSideQuests.Count == 0)
        {
            questListText.text = "There is no quest complete.";
        }
        else
        {
            questListText.text = "<b>Completed Main Quests:</b>\n";
            if (completedMainQuests.Count == 0)
            {
                questListText.text += "No completed main quests.\n\n";
            }
            else
            {
                foreach (var quest in completedMainQuests)
                {
                    questListText.text += $"<b>{quest.info.displayName}</b>:\n{quest.info.Description}\n\n";
                }
            }

            questListText.text += "<b>Completed Side Quests:</b>\n";
            if (completedSideQuests.Count == 0)
            {
                questListText.text += "No completed side quests.\n\n";
            }
            else
            {
                foreach (var quest in completedSideQuests)
                {
                    questListText.text += $"<b>{quest.info.displayName}</b>:\n{quest.info.Description}\n\n";
                }
            }
        }
    }
}
