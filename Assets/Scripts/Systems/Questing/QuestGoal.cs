using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Questing/Quest Goal")]
public class QuestGoal : ScriptableObject, IDataPersistence
{
    [SerializeField] protected string description;
    [SerializeField] private int currentAmount;
    [SerializeField] protected int requiredAmount;
    [SerializeField] private bool completed;
    [SerializeField] private QuestGoalType questGoalType;
    private Quest attachedQuest;
    
    public virtual void Init(Quest quest)
    {
        completed = false;
        currentAmount = 0;

        attachedQuest = quest;
        
        if (questGoalType == QuestGoalType.Combat)
        {
            GameEvents.Instance.onCombatVictory += OnCombatVictory;
            Debug.Log("Subscribed to CombatVictory");
        }
    }

    private void OnCombatVictory()
    {
        Debug.Log("Quest Goal func callback");
        currentAmount = Mathf.Clamp(currentAmount++, currentAmount, requiredAmount);
    }

    public bool IsComplete()
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

    public void LoadData(GameData data)
    {
        throw new NotImplementedException();
    }

    public void SaveData(GameData data)
    {
        throw new NotImplementedException();
    }
}
