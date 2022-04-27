using System;
using UnityEngine;

[Serializable]
public class CombatEncounterData
{
    [SerializeField] private int areaEncounterRate;
    [SerializeField] private int amountOfEnemiesToSpawn;
    [SerializeField] private int enemyLvl;

    public int AreaEncounterRate
    {
        get => areaEncounterRate;
        set => areaEncounterRate = value;
    }

    public int AmountOfEnemiesToSpawn
    {
        get => amountOfEnemiesToSpawn;
        set => amountOfEnemiesToSpawn = value;
    }

    public int EnemyLvl
    {
        get => enemyLvl;
        set => enemyLvl = value;
    }
}
