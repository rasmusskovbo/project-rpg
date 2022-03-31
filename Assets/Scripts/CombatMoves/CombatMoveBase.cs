using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatMove", menuName = "Combat/Create new Combat Move")]
public class CombatMoveBase : ScriptableObject
{
    [Header("Description")]
    [SerializeField] private string name;
    [TextArea] [SerializeField] private string description;

    [Header("Stats")] 
    [SerializeField] private CombatMoveType type;
    [SerializeField] private int power;
    [SerializeField] private int cooldown;
}
