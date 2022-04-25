using UnityEngine;

/*
 * Should be a singleton
 */
public class PlayerDataManager : SingletonPersistent<PlayerDataManager>
{
    private PlayerData playerData;              // Tracks global player related data such as exp, quests, items.
    private SettingsData settingsData;          // Tracks setting such as volume, difficulty etc
    private PlayerMovement playerMovementUnit;  // Tracks player positioning when exploring
    private CombatUnit playerCombatUnit;        // Tracks combat related stats such as hp, agility, attackpower
    private SkillManager skillManager;          // Tracks all active player skills and cooldowns
    private TalentManager talentManager;        // Tracks and handles player talents.

    [SerializeField] private CombatUnit playerCombatPrefab;

    private void Awake()
    {
        // Should be loaded. If nothing to load, initialize at lvl 1.
        if (playerCombatUnit == null)
        {
            playerCombatUnit = playerCombatPrefab;    
        }
    }

    public CombatUnit PlayerCombatUnit
    {
        get => playerCombatUnit;
        set => playerCombatUnit = value;
    }
}
