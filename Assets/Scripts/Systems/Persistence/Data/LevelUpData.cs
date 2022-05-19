using System;
using UnityEngine;

[Serializable]
public class LevelUpData : SaveData
{
    [SerializeField] private int statPointsPrLevel;
    [SerializeField] private int capBaseGrowth;
    [SerializeField] private int capGrowthRate;
    [SerializeField] private float capMultiplier;

    public int StatPointsPrLevel
    {
        get => statPointsPrLevel;
        set => statPointsPrLevel = value;
    }

    public int CapBaseGrowth
    {
        get => capBaseGrowth;
        set => capBaseGrowth = value;
    }

    public int CapGrowthRate
    {
        get => capGrowthRate;
        set => capGrowthRate = value;
    }

    public float CapMultiplier
    {
        get => capMultiplier;
        set => capMultiplier = value;
    }

    public void ResetBeforeSave()
    {
        throw new NotImplementedException();
    }
}
