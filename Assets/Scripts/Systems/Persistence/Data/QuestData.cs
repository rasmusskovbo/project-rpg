using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestData : SaveData
{
    [SerializeField] private List<Quest> activeQuests;
    [SerializeField] private SerializableDictionary<QuestGoal, int> questGoalProgress;

    public QuestData(List<Quest> activeQuests, SerializableDictionary<QuestGoal, int> questGoalProgress)
    {
        this.activeQuests = activeQuests;
        this.questGoalProgress = questGoalProgress;
    }

    public List<Quest> ActiveQuests
    {
        get => activeQuests;
        set => activeQuests = value;
    }
    
    public SerializableDictionary<QuestGoal, int> QuestGoalProgress
    {
        get => questGoalProgress;
        set => questGoalProgress = value;
    }
    
    public void ResetBeforeSave()
    {
        activeQuests = new List<Quest>();
        questGoalProgress = new SerializableDictionary<QuestGoal, int>();
    }
}
