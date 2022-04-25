using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Combat/Create new Enemy SO")]
public class EnemySO : ScriptableObject
{
    [Header("Description")]
    [SerializeField] private string unitName;
    [TextArea] [SerializeField] private string description;
    [SerializeField] private UnitType unitType;
    
    [Header("VFX")] 
    [SerializeField] private Sprite idleSprite;
    
    [Header("Base Stats")]
    [SerializeField] private int maxHp;
    [SerializeField] private float strength;
    [SerializeField] private float agility;
    [SerializeField] private float intellect;
    [SerializeField] private float attackPower;
    [SerializeField] private float abilityPower;
    [SerializeField] private float physicalCritChance;
    [SerializeField] private float magicalCritChance;
    [SerializeField] private float physicalDefense;
    [SerializeField] private float magicalDefense;
    [SerializeField] private float physicalBlockPower;
    [SerializeField] private float dodgeChance;
    [SerializeField] private float speed;

    [Header("Stat Ratios")] 
    [SerializeField] private float strengthApRatio = 2;
    [SerializeField] private float agilityApRatio = 1;
    [SerializeField] private float intellectAbpRatio = 2;
    [SerializeField] private float agilityCritRatio = 0.05f;
    [SerializeField] private float intellectCritRatio = 0.05f;
    [SerializeField] private float strengthPhysDefRatio = 1;
    [SerializeField] private float agilityPhysDefRatio = 2;
    [SerializeField] private float agilityDodgeRatio = 0.05f;
    [SerializeField] private float agilitySpeedRatio = 1;
    [SerializeField] private float critMultiplier = 2;
    
    [Header("Growth Rates")]
    [SerializeField] private float maxHpGrowth = 2;
    [SerializeField] private float strengthGrowth = 1;
    [SerializeField] private float agilityGrowth = 1;
    [SerializeField] private float intellectGrowth = 1;
    [SerializeField] private float physicalDefenseGrowth = 1;
    [SerializeField] private float magicalDefenseGrowth = 2;
    [SerializeField] private float physicalBlockPowerGrowth = 1;
    [SerializeField] private float speedGrowth = 2;

    public string UnitName
    {
        get => unitName;
        set => unitName = value;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }

    public UnitType UnitType
    {
        get => unitType;
        set => unitType = value;
    }

    public Sprite IdleSprite
    {
        get => idleSprite;
        set => idleSprite = value;
    }

    public int MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }

    public float Strength
    {
        get => strength;
        set => strength = value;
    }

    public float Agility
    {
        get => agility;
        set => agility = value;
    }

    public float Intellect
    {
        get => intellect;
        set => intellect = value;
    }

    public float AttackPower
    {
        get => attackPower;
        set => attackPower = value;
    }

    public float AbilityPower
    {
        get => abilityPower;
        set => abilityPower = value;
    }

    public float PhysicalCritChance
    {
        get => physicalCritChance;
        set => physicalCritChance = value;
    }

    public float MagicalCritChance
    {
        get => magicalCritChance;
        set => magicalCritChance = value;
    }

    public float PhysicalDefense
    {
        get => physicalDefense;
        set => physicalDefense = value;
    }

    public float MagicalDefense
    {
        get => magicalDefense;
        set => magicalDefense = value;
    }

    public float PhysicalBlockPower
    {
        get => physicalBlockPower;
        set => physicalBlockPower = value;
    }

    public float DodgeChance
    {
        get => dodgeChance;
        set => dodgeChance = value;
    }

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public float StrengthApRatio
    {
        get => strengthApRatio;
        set => strengthApRatio = value;
    }

    public float AgilityApRatio
    {
        get => agilityApRatio;
        set => agilityApRatio = value;
    }

    public float IntellectAbpRatio
    {
        get => intellectAbpRatio;
        set => intellectAbpRatio = value;
    }

    public float AgilityCritRatio
    {
        get => agilityCritRatio;
        set => agilityCritRatio = value;
    }

    public float IntellectCritRatio
    {
        get => intellectCritRatio;
        set => intellectCritRatio = value;
    }

    public float StrengthPhysDefRatio
    {
        get => strengthPhysDefRatio;
        set => strengthPhysDefRatio = value;
    }

    public float AgilityPhysDefRatio
    {
        get => agilityPhysDefRatio;
        set => agilityPhysDefRatio = value;
    }

    public float AgilityDodgeRatio
    {
        get => agilityDodgeRatio;
        set => agilityDodgeRatio = value;
    }

    public float AgilitySpeedRatio
    {
        get => agilitySpeedRatio;
        set => agilitySpeedRatio = value;
    }

    public float CritMultiplier
    {
        get => critMultiplier;
        set => critMultiplier = value;
    }

    public float MaxHpGrowth
    {
        get => maxHpGrowth;
        set => maxHpGrowth = value;
    }

    public float StrengthGrowth
    {
        get => strengthGrowth;
        set => strengthGrowth = value;
    }

    public float AgilityGrowth
    {
        get => agilityGrowth;
        set => agilityGrowth = value;
    }

    public float IntellectGrowth
    {
        get => intellectGrowth;
        set => intellectGrowth = value;
    }

    public float PhysicalDefenseGrowth
    {
        get => physicalDefenseGrowth;
        set => physicalDefenseGrowth = value;
    }

    public float MagicalDefenseGrowth
    {
        get => magicalDefenseGrowth;
        set => magicalDefenseGrowth = value;
    }

    public float PhysicalBlockPowerGrowth
    {
        get => physicalBlockPowerGrowth;
        set => physicalBlockPowerGrowth = value;
    }

    public float SpeedGrowth
    {
        get => speedGrowth;
        set => speedGrowth = value;
    }
}
