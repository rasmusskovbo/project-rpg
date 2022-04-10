using UnityEngine;

// Enemy scriptable object
[CreateAssetMenu(fileName = "Enemy", menuName = "Combat/Create new Enemy")]
public class EnemyBase : ScriptableObject
{
    [Header("Description")]
    [SerializeField] private string name;
    [TextArea] [SerializeField] private string description;
    
    [Header("Base Stats")]
    [SerializeField] private int maxHp;
    [SerializeField] private int level;
    [SerializeField] private float attackPower;
    [SerializeField] private float abilityPower;
    [SerializeField] private float physicalDefense;
    [SerializeField] private float magicalDefense;
    [SerializeField] private float physicalBlockPower;
    [SerializeField] private float dodge;
    [SerializeField] private float speed;

    [Header("VFX")] 
    [SerializeField] private Sprite idleSprite;

    // Properties/Get+Setters
    public string Name
    {
        get => name;
        set => name = value;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }

    public int MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }

    public int Level
    {
        get => level;
        set => level = value;
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

    public float Dodge
    {
        get => dodge;
        set => dodge = value;
    }

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    public Sprite IdleSprite
    {
        get => idleSprite;
        set => idleSprite = value;
    }
}
