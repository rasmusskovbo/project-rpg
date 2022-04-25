using System;
using UnityEngine;

[Serializable]
public class GameData
{
    [SerializeField] public PlayerData playerData;
    [SerializeField] private SettingsData settingsData;
    [SerializeField] private SkillData skillData;

    public GameData(PlayerData playerData, SettingsData settingsData, SkillData skillData)
    {
        this.playerData = playerData;
        this.settingsData = settingsData;
        this.skillData = skillData;
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
}