using System;
using UnityEngine;

[Serializable]
public class NPC : MonoBehaviour, Interactable
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Quest quest;
    [SerializeField] private bool questHasBeenTurnedIn;
    private QuestManager questManager;
    
    public void Interact()
    {
        Debug.Log("Interacting with NPC");
        
        if (questHasBeenTurnedIn)
        {
            // Play post-turning in quest dialogue here.
            Debug.Log("This quest has already been turned in");
            return;
        }

        if (questManager == null) questManager = FindObjectOfType<QuestManager>();
        bool isQuestActive = questManager.ActiveQuests.Contains(quest);
        bool isQuestCompleted = quest.IsQuestComplete();
        
        if (!isQuestActive)
        {
            // Quest intro dialogue here
            FindObjectOfType<DialogueManager>().ShowDialog(dialogue);
            FindObjectOfType<QuestManager>().AddQuest(quest);
        }
        else if (isQuestActive && isQuestCompleted)
        {
            // Turn in the quest and get rewards:
            // Quest completion dialogue here
            Debug.Log("Completing quest");
            questManager.CompleteQuest(quest);
            questHasBeenTurnedIn = true;
        }
    }
}

