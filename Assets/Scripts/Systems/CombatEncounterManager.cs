using UnityEngine;

public class CombatEncounterManager : PersistentSingleton<CombatEncounterManager>, IDataPersistence
{
    [SerializeField] int areaEncounterRate = 20;
    [SerializeField] private int amountOfEnemiesToSpawn = 1;
    [SerializeField] private int enemyLvl = 1;

    // Updating encounter stats logic here.
    
    public void LoadData(GameData data)
    {
        areaEncounterRate = data.CombatEncounterData.AreaEncounterRate;
        amountOfEnemiesToSpawn = data.CombatEncounterData.AmountOfEnemiesToSpawn;
        enemyLvl = data.CombatEncounterData.EnemyLvl;
    }

    public void SaveData(GameData data)
    {
        data.CombatEncounterData.AreaEncounterRate = areaEncounterRate;
        data.CombatEncounterData.AmountOfEnemiesToSpawn = amountOfEnemiesToSpawn;
        data.CombatEncounterData.EnemyLvl = enemyLvl;
    }

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
