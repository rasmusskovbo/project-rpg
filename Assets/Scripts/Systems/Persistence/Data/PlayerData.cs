using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    // Progress stats
    [SerializeField] private int level;
    [SerializeField] private int exp;
    [SerializeField] private int requiredExpForLevelUp;
    
    // Exploration stats
    [SerializeField] private int sceneIndex;
    [SerializeField] private Vector3 position;
    [SerializeField] private PlayerFacing playerFacingDirection;
    
    // Combat Stats
    [SerializeField] private float currentHp;
    //private CombatEffectManager effectManager;

    public PlayerData(int level, int exp, int requiredExpForLevelUp, int sceneIndex, Vector3 position, PlayerFacing playerFacingDirection, float currentHp)
    {
        this.level = level;
        this.exp = exp;
        this.requiredExpForLevelUp = requiredExpForLevelUp;
        this.sceneIndex = sceneIndex;
        this.position = position;
        this.playerFacingDirection = playerFacingDirection;
        this.currentHp = currentHp;
    }

    public int Level
    {
        get => level;
        set => level = value;
    }

    public int Exp
    {
        get => exp;
        set => exp = value;
    }

    public int RequiredExpForLevelUp
    {
        get => requiredExpForLevelUp;
        set => requiredExpForLevelUp = value;
    }

    public int SceneIndex
    {
        get => sceneIndex;
        set => sceneIndex = value;
    }

    public Vector3 Position
    {
        get => position;
        set => position = value;
    }

    public PlayerFacing PlayerFacingDirection
    {
        get => playerFacingDirection;
        set => playerFacingDirection = value;
    }

    public float CurrentHp
    {
        get => currentHp;
        set => currentHp = value;
    }
}
