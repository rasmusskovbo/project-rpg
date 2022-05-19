using System;
using UnityEngine;

[Serializable]
public class NPC : MonoBehaviour, Interactable, IDataPersistence
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Quest quest;
    [SerializeField] private bool questHasBeenTurnedIn;
    [SerializeField] private string npcId;
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

    public void LoadData(GameData data)
    {
        if (data.NpcData.npcQuestTurnedInMap.ContainsKey(npcId)) 
            this.questHasBeenTurnedIn = data.NpcData.npcQuestTurnedInMap[npcId];
    }

    public void SaveData(GameData data)
    {
        data.NpcData.ResetBeforeSave();
        data.NpcData.npcQuestTurnedInMap.Add(npcId, this.questHasBeenTurnedIn);
    }
}

