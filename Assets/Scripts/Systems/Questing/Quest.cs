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
    [SerializeField] private string questId;

    public void Initialize()
    {
        foreach (QuestGoal goal in goals)
        {
            goal.Init(this);
        }
    }
    
    public bool IsQuestComplete()
    {
        return goals.All(goal => goal.IsComplete());
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
    }

    public List<QuestGoal> Goals
    {
        get => goals;
        set => goals = value;
    }

    
}
