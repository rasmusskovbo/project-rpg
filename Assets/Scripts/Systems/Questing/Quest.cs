using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Questing/Quest")]
public class Quest : ScriptableObject
{
    [Header("Info")] [SerializeField] public Info info;
    [Header("Reward")] [SerializeField] public Reward reward;
    [SerializeField] private List<QuestGoal> goals;
    [SerializeField] private bool completed;

    public void Initialize()
    {
        completed = false;

        foreach (QuestGoal goal in goals)
        {
            goal.Init();
        }
    }

    // Check this intermittently (fx everytime quest window is opened or an event is invoked)
    private void CheckGoals()
    {
        completed = goals.All(goal => goal.Completed);

        if (completed)
        {
            // Give rewards
            // Destroy/remove quests
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

    public List<QuestGoal> Goals
    {
        get => goals;
        set => goals = value;
    }
    
}
