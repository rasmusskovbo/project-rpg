using System;
using UnityEngine;

[Serializable]
public class PlayerData : SaveData
{
    // Progress stats
    [SerializeField] public int level;
    [SerializeField] public int exp;
    [SerializeField] public int nextLvLExp;
    [SerializeField] public int remainingStatPoints;

    // Exploration stats
    [SerializeField] public int sceneIndex;
    [SerializeField] public Vector3 position;
    [SerializeField] public PlayerFacing playerFacingDirection;

    // Combat Stats
    [SerializeField] public UnitBase unitBase;
    [SerializeField] public float currentHp;
    //private CombatEffectManager effectManager;

    public PlayerData(int level, int exp, int nextLvLExp, int remainingStatPoints, int sceneIndex, Vector3 position, PlayerFacing playerFacingDirection, UnitBase unitBase, float currentHp)
    {
        this.level = level;
        this.exp = exp;
        this.nextLvLExp = nextLvLExp;
        this.remainingStatPoints = remainingStatPoints;
        this.sceneIndex = sceneIndex;
        this.position = position;
        this.playerFacingDirection = playerFacingDirection;
        this.unitBase = unitBase;
        this.currentHp = currentHp;
    }

    public void ResetBeforeSave()
    {
        throw new NotImplementedException();
    }
}
