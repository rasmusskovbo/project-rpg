using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CombatMove", menuName = "Combat/Create new Combat Move")]
public class CombatMoveBase : ScriptableObject
{
    [Header("Description")]
    [SerializeField] private string name;
    [TextArea] [SerializeField] private string description;

    [Header("Stats")] 
    [SerializeField] private CombatMoveTypeSO type;
    [SerializeField] private int power;
    [SerializeField] private int cooldown;

    [Header("VFX")] 
    [SerializeField] private Sprite icon_image;

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

    public CombatMoveTypeSO Type
    {
        get => type;
        set => type = value;
    }

    public int Power
    {
        get => power;
        set => power = value;
    }

    public int Cooldown
    {
        get => cooldown;
        set => cooldown = value;
    }

    public Sprite IconImage
    {
        get => icon_image;
        set => icon_image = value;
    }
    
}
