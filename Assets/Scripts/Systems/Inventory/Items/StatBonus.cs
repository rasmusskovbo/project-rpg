using System;
using UnityEngine;

[Serializable]
public class StatBonus
{
    [SerializeField] private StatBonusType statBonusType;
    [SerializeField] private float value;

    public StatBonus(StatBonusType statBonusType, float value)
    {
        this.statBonusType = statBonusType;
        this.value = value;
    }

    public StatBonusType StatBonusType
    {
        get => statBonusType;
        set => statBonusType = value;
    }

    public float Value
    {
        get => value;
        set => this.value = value;
    }
}
