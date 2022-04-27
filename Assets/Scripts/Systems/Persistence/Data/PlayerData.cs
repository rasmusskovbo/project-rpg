using System;
using UnityEngine;

[Serializable]
public class PlayerData
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
    [SerializeField] public SerializableDictionary<string, bool> chestsCollected;

    // Combat Stats
    [SerializeField] public UnitBase unitBase;
    [SerializeField] public float currentHp;
    //private CombatEffectManager effectManager;

    public PlayerData(int level, int exp, int nextLvLExp, int remainingStatPoints, int sceneIndex, Vector3 position, PlayerFacing playerFacingDirection, SerializableDictionary<string, bool> chestsCollected, UnitBase unitBase)
    {
        this.level = level;
        this.exp = exp;
        this.nextLvLExp = nextLvLExp;
        this.remainingStatPoints = remainingStatPoints;
        this.sceneIndex = sceneIndex;
        this.position = position;
        this.playerFacingDirection = playerFacingDirection;
        this.chestsCollected = chestsCollected;
        this.unitBase = unitBase;
        this.currentHp = unitBase.MaxHp;
    }
}
