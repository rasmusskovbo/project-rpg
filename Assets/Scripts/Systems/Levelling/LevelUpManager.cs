using UnityEngine;

public class LevelUpManager : PersistentSingleton<LevelUpManager>, IDataPersistence
{
    private GameManager gameManager;

    [SerializeField] private int statPointsPrLevel = 3;
    [SerializeField] private int capBaseGrowth = 15;
    [SerializeField] private int capGrowthRate = 10;
    [SerializeField] private float capMultiplier = 1.1f;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public bool PlayerShouldLevelUp(int currentExp, int nextLvlExp)
    {
        Debug.Log("XP: " + currentExp + ", " + nextLvlExp);
        return currentExp >= nextLvlExp;
    }

    public void LevelUp()
    {
        Debug.Log("levelling up");
        gameManager.PlayerData.remainingStatPoints += statPointsPrLevel;
        gameManager.PlayerData.nextLvLExp += capBaseGrowth;

        capGrowthRate = Mathf.RoundToInt(capGrowthRate * capMultiplier);
        capBaseGrowth += capGrowthRate;
    }

    public void LoadData(GameData data)
    {
        statPointsPrLevel = data.LevelUpData.StatPointsPrLevel;
        capBaseGrowth = data.LevelUpData.CapBaseGrowth;
        capGrowthRate = data.LevelUpData.CapGrowthRate;
        capMultiplier = data.LevelUpData.CapMultiplier;
    }

    public void SaveData(GameData data)
    {
        data.LevelUpData.StatPointsPrLevel = statPointsPrLevel;
        data.LevelUpData.CapBaseGrowth = capBaseGrowth;
        data.LevelUpData.CapGrowthRate = capGrowthRate;
        data.LevelUpData.CapMultiplier = capMultiplier;
    }
}
