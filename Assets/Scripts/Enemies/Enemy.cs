using UnityEngine;

public class Enemy : Unit
{
    private EnemyBase _base;

    public void InitiateEnemy(EnemyBase scriptableObject, int level)
    {
        _base = scriptableObject;
        Level = level;
        GetComponent<SpriteRenderer>().sprite = _base.IdleSprite;
        InitiateStaticStats();
        InitiateCurrentStats();
    }
    
    // Growth multipliers
    [SerializeField] private float maxHpGrowth = 10;
    [SerializeField] private float attackPowerGrowth = 5;
    [SerializeField] private float abilityPowerGrowth = 5;
    [SerializeField] private float physicalDefenseGrowth = 5;
    [SerializeField] private float magicalDefenseGrowth = 5;
    [SerializeField] private float physicalBlocKPowerGrowth = 1;
    [SerializeField] private float dodgeGrowth = 5;
    [SerializeField] private float speedGrowth = 5;
    
    // Load enemy scriptable object's stats into base Unit stats.
    public void InitiateStaticStats()
    {
        UnitName = _base.name;
        MaxHp = Mathf.FloorToInt(((_base.MaxHp * Level) / 100f) + maxHpGrowth);
        AttackPower = Mathf.FloorToInt(((_base.AttackPower * Level) / 100f) + attackPowerGrowth);
        AbilityPower = Mathf.FloorToInt(((_base.AbilityPower * Level) / 100f) + abilityPowerGrowth);
        PhysicalDefense = Mathf.FloorToInt(((_base.PhysicalDefense * Level) / 100f) + physicalDefenseGrowth);
        MagicalDefense = Mathf.FloorToInt(((_base.MagicalDefense * Level) / 100f) + magicalDefenseGrowth);
        PhysicalBlockPower = Mathf.FloorToInt(((_base.PhysicalBlockPower * Level) / 100f) + physicalBlocKPowerGrowth);
        Dodge = Mathf.FloorToInt(((_base.Dodge * Level) / 100f) + dodgeGrowth);
        Speed = Mathf.FloorToInt(((_base.Speed * Level) / 100f) + speedGrowth);
    }
    
}
