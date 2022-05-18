using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField] private List<Quest> activeQuests;

    private void Start()
    {
        activeQuests.ForEach(quest => quest.Initialize());
    }

    public void AddQuest(Quest quest)
    {
        quest.Initialize();
        activeQuests.Add(quest);
        UIQuestController.Instance.CreateOrUpdateSlot(quest);
    }

    public void RemoveQuest(Quest quest)
    {
        activeQuests.Remove(quest);
        UIQuestController.Instance.RemoveQuestFromUI(quest);
    }

    public void CompleteQuest(Quest quest)
    {
        Debug.Log("Completing quest: " + quest.info.Name);
        // Handle other types of rewarsd here.
        if (quest.reward.type == RewardType.Experience)
        {
            FindObjectOfType<GameManager>().PlayerData.exp += quest.reward.amount;
        }
        
        RemoveQuest(quest);
    }
    
    public List<Quest> ActiveQuests
    {
        get => activeQuests;
        set => activeQuests = value;
    }
}
