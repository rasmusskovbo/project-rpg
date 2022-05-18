using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>, IDataPersistence
{
    [SerializeField] private PlayerData playerData;              // Tracks global player related data such as exp, quests, items.
    [SerializeField] private SettingsData settingsData;          // Tracks setting such as volume, difficulty etc
    [SerializeField] private SkillData skillData;                // Tracks all active player skills and cooldowns
    //private TalentData talentData;                            // Tracks and handles player talents.

    // References
    private PlayerController playerControllerUnit;
    private LevelUpManager levelUpManager;
    private DialogueManager dialogueManager;
    
    // Expl State
    [SerializeField] private ExplorationState explorationState;

    private void Start()
    {
        playerControllerUnit = FindObjectOfType<PlayerController>();
        levelUpManager = FindObjectOfType<LevelUpManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        explorationState = ExplorationState.Explore;

        GameEvents.Instance.onShowDialog += () =>
        {
            explorationState = ExplorationState.Dialog;
        };
        GameEvents.Instance.onCloseDialog += () =>
        {
            if (explorationState == ExplorationState.Dialog) explorationState = ExplorationState.Explore;
        };
    }
    
    public void UpdatePlayerDataAfterCombat(CombatResult result)
    {
        playerData.currentHp = result.PlayerCurrentHp;
        playerData.exp += result.XpGained;
        if (levelUpManager.PlayerShouldLevelUp(playerData.exp, playerData.nextLvLExp)) levelUpManager.LevelUp();
    }

    public void SavePositionBeforeCombat()
    {
        playerControllerUnit = FindObjectOfType<PlayerController>();
        
        playerData.position = playerControllerUnit.transform.position;
        playerData.playerFacingDirection = playerControllerUnit.PlayerFacingDirection;
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

    public ExplorationState ExplorationState
    {
        get => explorationState;
        set => explorationState = value;
    }
}
