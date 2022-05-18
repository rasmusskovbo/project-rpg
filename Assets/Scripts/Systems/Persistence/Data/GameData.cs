using System;
using UnityEngine;

[Serializable]
public class GameData
{
    [SerializeField] public PlayerData playerData;
    [SerializeField] private SettingsData settingsData;
    [SerializeField] private SkillData skillData;
    [SerializeField] private LevelUpData levelUpData;
    [SerializeField] private InventoryData inventoryData;
    [SerializeField] private EquipmentData equipmentData;
    [SerializeField] private CombatEncounterData combatEncounterData;
    [SerializeField] private NPCData npcData;

    public GameData(PlayerData playerData, SettingsData settingsData, SkillData skillData, LevelUpData levelUpData, InventoryData inventoryData, EquipmentData equipmentData, CombatEncounterData combatEncounterData, NPCData npcData)
    {
        this.playerData = playerData;
        this.settingsData = settingsData;
        this.skillData = skillData;
        this.levelUpData = levelUpData;
        this.inventoryData = inventoryData;
        this.equipmentData = equipmentData;
        this.combatEncounterData = combatEncounterData;
        this.npcData = npcData;
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

    public InventoryData InventoryData
    {
        get => inventoryData;
        set => inventoryData = value;
    }

    public EquipmentData EquipmentData
    {
        get => equipmentData;
        set => equipmentData = value;
    }

    public CombatEncounterData CombatEncounterData
    {
        get => combatEncounterData;
        set => combatEncounterData = value;
    }

    public NPCData NpcData
    {
        get => npcData;
        set => npcData = value;
    }
}