using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Questing/Quest")]
public class Quest : ScriptableObject
{
    [Header("Info")] [SerializeField] private Info info;
    [Header("Reward")] [SerializeField] private Reward reward;
    [SerializeField] private List<QuestGoal> goals;
    [SerializeField] public bool Completed { get; protected set; }
    [SerializeField] public QuestCompletedEvent questCompleted;

    public void Initialize()
    {
        Completed = false;
        questCompleted = new QuestCompletedEvent();

        foreach (QuestGoal goal in goals)
        {
            goal.Init();
            goal.GoalCompleted.AddListener(() => CheckGoals());
        }
    }

    private void CheckGoals()
    {
        Completed = goals.All(goal => goal.Completed);

        if (Completed)
        {
            // Give reward here
            questCompleted.Invoke(this);
            questCompleted.RemoveAllListeners();
        }
    }

    [Serializable]
    public struct Info
    {
        [SerializeField] public string Name;
        [SerializeField] public Sprite Icon;
        [SerializeField] public string Description;
    }
    
    [Serializable]
    public struct Reward
    {
        [SerializeField] public RewardType type;
        [SerializeField] public int amount;
        
        [Serializable]
        public enum RewardType
        {
            Currency,
            Experience,
            Item
        }
    }
    
}
