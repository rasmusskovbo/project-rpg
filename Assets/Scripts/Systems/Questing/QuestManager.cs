using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>, IDataPersistence
{
    [SerializeField] private List<Quest> activeQuests;
    
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
        // Handle other types of rewards here.
        if (quest.reward.type == RewardType.Experience)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.PlayerData.exp += quest.reward.amount;
            gameManager.CheckForLevelUpAfterQuest();
        }
        
        RemoveQuest(quest);
    }
    
    public List<Quest> ActiveQuests
    {
        get => activeQuests;
        set => activeQuests = value;
    }

    public void LoadData(GameData data)
    {
        data.QuestData.ActiveQuests.ForEach(quest =>
        {
            quest.Goals.ForEach(goal =>
            {
                goal.Init(quest);
                goal.CurrentAmount = data.QuestData.QuestGoalProgress[goal];
            });
            
            ActiveQuests.Add(quest);
        });
    }

    public void SaveData(GameData data)
    {
        data.QuestData.ResetBeforeSave();
        
        ActiveQuests.ForEach(quest =>
        {
            quest.Goals.ForEach(goal =>
            {
                if (data.QuestData.QuestGoalProgress.ContainsKey(goal))
                {
                    data.QuestData.QuestGoalProgress[goal] = goal.CurrentAmount;
                }
                else
                {
                    data.QuestData.QuestGoalProgress.Add(goal, goal.CurrentAmount);    
                }
            });

            if (!data.QuestData.ActiveQuests.Contains(quest)) data.QuestData.ActiveQuests.Add(quest);
        });
    }
    
    
}
