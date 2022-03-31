using Enemies;
using UnityEngine;

public class Enemy
{
    private EnemyBase _base;
    private int level;
    
    public Enemy(EnemyBase eBase, int eLevel)
    {
        _base = eBase;
        level = eLevel;
    }
    
    // Growth multipliers
    private float maxHpGrowth = 10;
    private float attackPowerGrowth = 5;
    private float abilityPowerGrowth = 5;
    private float physicalDefenseGrowth = 5;
    private float magicalDefenseGrowth = 5;
    private float dodgeGrowth = 5;
    private float speedGrowth = 5;
    
    // Getters after multipliers
    public int Level
    {
        get => level;
    }

    public int MaxHp
    {
        get => Mathf.FloorToInt(((_base.MaxHp * level) / 100f) + maxHpGrowth);
    }
    
    public float AttackPower
    {
        get => Mathf.FloorToInt(((_base.AttackPower * level) / 100f) + attackPowerGrowth);
    }

    public float AbilityPower
    {
        get => Mathf.FloorToInt(((_base.AbilityPower * level) / 100f) + abilityPowerGrowth);
    }

    public float PhysicalDefense
    {
        get => Mathf.FloorToInt(((_base.PhysicalDefense * level) / 100f) + physicalDefenseGrowth);
    }

    public float MagicalDefense
    {
        get => Mathf.FloorToInt(((_base.MagicalDefense * level) / 100f) + magicalDefenseGrowth);
    }
    
    public float Dodge
    {
        get => Mathf.FloorToInt(((_base.Dodge * level) / 100f) + dodgeGrowth);
    }

    public float Speed
    {
        get => Mathf.FloorToInt(((_base.Speed * level) / 100f) + speedGrowth);
    }
    
    // Combat tracking stats:
    private float currentHp;
    private float currentMaxHp;
    private float currentAttackPower;
    private float currentAbilityPower;
    private float currentPhysDef;
    private float currentMagicDef;
    private float currentSpeed;
    private CombatStatus _status = CombatStatus.None;
    
}
