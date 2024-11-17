using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestMenu : MonoBehaviour
{
    [SerializeField] public GameObject questMenuPanel;
    [SerializeField] private Button mainQuestTab;
    [SerializeField] private Button sideQuestTab;
    [SerializeField] private Button completedQuestTab; // Nouveau bouton pour les quêtes terminées
    [SerializeField] private TextMeshProUGUI questListText;
    
    private bool isMenuVisible = false;
    private bool showingMainQuests = true;
    private bool showingCompletedQuests = false;

    public QuestManager questManager;
    public SideQuestManager sideQuestManager;

    private void Start()
    {
        questMenuPanel.SetActive(false);

        mainQuestTab.onClick.AddListener(ShowMainQuests);
        sideQuestTab.onClick.AddListener(ShowSideQuests);
        completedQuestTab.onClick.AddListener(ShowCompletedQuests); // Ajoute un écouteur pour le bouton

        UpdateQuestDisplay();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
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
        foreach (var quest in mainQuests)
        {
            questListText.text += $"<b>{quest.info.displayName}</b>:\n{quest.info.Description}\n\n";
        }
    }

    private void DisplaySideQuests()
    {
        var sideQuests = sideQuestManager.GetActiveSideQuests();
        foreach (var quest in sideQuests)
        {
            questListText.text += $"<b>{quest.info.displayName}</b>:\n{quest.info.Description}\n\n";
        }
    }

    private void DisplayCompletedQuests()
    {
        var completedQuests = questManager.GetCompletedQuests();
        foreach (var quest in completedQuests)
        {
            questListText.text += $"<b>{quest.info.displayName}</b>:\n{quest.info.Description}\n\n";
        }
    }
}
