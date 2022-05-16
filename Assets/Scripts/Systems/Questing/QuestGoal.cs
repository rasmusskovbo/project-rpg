using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
[CreateAssetMenu(menuName = "Questing/Quest Goal")]
public class QuestGoal : ScriptableObject
{
    [SerializeField] protected string description;
    [SerializeField] private int currentAmount;
    [SerializeField] protected int requiredAmount = 1;
    [SerializeField] private bool completed;
    [SerializeField] private QuestGoalType questGoalType;
    
    public virtual void Init()
    {
        completed = false;
        
        if (questGoalType == QuestGoalType.Combat)
        {
            GameEvents.Instance.onCombatVictory += OnCombatVictory;
            Debug.Log("Subscribed to CombatVictory");
        }
    }

    private void OnCombatVictory()
    {
        Debug.Log("Quest Goal func callback");
        currentAmount++;
        // Maybe call completion check here
    }

    protected bool isComplete()
    {
        return currentAmount >= requiredAmount;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }

    public int CurrentAmount
    {
        get => currentAmount;
        set => currentAmount = value;
    }

    public int RequiredAmount
    {
        get => requiredAmount;
        set => requiredAmount = value;
    }

    public bool Completed
    {
        get => completed;
        set => completed = value;
    }
    
}
