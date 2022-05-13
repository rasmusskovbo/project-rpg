using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class QuestGoal : ScriptableObject
{
    [SerializeField] protected string description;
    [SerializeField] public int CurrentAmount { get; protected set; }
    [SerializeField] public int RequiredAmount = 1;
    
    [SerializeField] public bool Completed { get; protected set; }
    [SerializeField][HideInInspector] public UnityEvent GoalCompleted;
    
    public virtual string Description()
    {
        return description;
    }

    public virtual void Init()
    {
        Completed = false;
        GoalCompleted = new UnityEvent();
    }

    protected void Evaluate()
    {
        if (CurrentAmount >= RequiredAmount)
        {
            Complete();
        }
    }

    private void Complete()
    {
        Completed = true;
        GoalCompleted.Invoke();
        GoalCompleted.RemoveAllListeners();
    }
}
