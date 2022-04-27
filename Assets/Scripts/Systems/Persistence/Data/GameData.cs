using System;
using UnityEngine;

[Serializable]
public class GameData
{
    [SerializeField] public PlayerData playerData;
    [SerializeField] private SettingsData settingsData;
    [SerializeField] private SkillData skillData;
    [SerializeField] private LevelUpData levelUpData;
    [SerializeField] private CombatEncounterData combatEncounterData;

    public GameData(PlayerData playerData, SettingsData settingsData, SkillData skillData, LevelUpData levelUpData, CombatEncounterData combatEncounterData)
    {
        this.playerData = playerData;
        this.settingsData = settingsData;
        this.skillData = skillData;
        this.levelUpData = levelUpData;
        this.combatEncounterData = combatEncounterData;
    }

    public PlayerData PlayerData
    {
        get => playerData;
        set => playerData = value;
    }

    public SettingsData SettingsData
    {
        get => settingsData;
        set => settingsData = value;
    }

    public SkillData SkillData
    {
        get => skillData;
        set => skillData = value;
    }

    public LevelUpData LevelUpData
    {
        get => levelUpData;
        set => levelUpData = value;
    }

    public CombatEncounterData CombatEncounterData
    {
        get => combatEncounterData;
        set => combatEncounterData = value;
    }
}