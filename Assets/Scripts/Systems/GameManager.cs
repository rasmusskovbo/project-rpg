/*
 * This object tracks all live stats.
 */
public class GameManager : PersistentSingleton<GameManager>, IDataPersistence
{
    private PlayerData playerData;              // Tracks global player related data such as exp, quests, items.
    private SettingsData settingsData;          // Tracks setting such as volume, difficulty etc
    private SkillData skillData;                // Tracks all active player skills and cooldowns
    //private TalentData talentData;        // Tracks and handles player talents.

    // References
    private PlayerMovement playerMovementUnit;
    private LevelUpManager levelUpManager;

    private void Start()
    {
        playerMovementUnit = FindObjectOfType<PlayerMovement>();
        levelUpManager = FindObjectOfType<LevelUpManager>();
    }
    
    public void UpdatePlayerDataAfterCombat(CombatResult result)
    {
        playerData.currentHp = result.PlayerCurrentHp;
        playerData.exp += result.XpGained;
        if (levelUpManager.PlayerShouldLevelUp(playerData.exp, playerData.nextLvLExp)) levelUpManager.LevelUp();
    }

    public void SavePositionBeforeCombat()
    {
        playerMovementUnit = FindObjectOfType<PlayerMovement>();
        
        playerData.position = playerMovementUnit.transform.position;
        playerData.playerFacingDirection = playerMovementUnit.PlayerFacingDirection;
    }
    
    public void AddStatPoint(StatType type)
    {
        switch (type)
        {
            case StatType.Strength:
                PlayerData.unitBase.Strength++;
                break;
            case StatType.Agility:
                PlayerData.unitBase.Agility++;
                break;
            case StatType.Intellect:
                PlayerData.unitBase.Intellect++;
                break;
        }

        playerData.remainingStatPoints--;
    }
    
    public PlayerData PlayerData
    {
        get => playerData;
    }
    
    public void LoadData(GameData data)
    {
        playerData = data.PlayerData;
        settingsData = data.SettingsData;
        skillData = data.SkillData;
    }

    public void SaveData(GameData data)
    {
        data.PlayerData = playerData;
        data.SettingsData = settingsData;
        data.SkillData = skillData;
    }
}
