
using Enemies;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatMoveType", menuName = "Combat/Create new Type SO")]
public class CombatMoveTypeSO : ScriptableObject
{
    [SerializeField] private CombatMoveType type;
    [SerializeField] private Sprite icon;
    
    public CombatMoveType GetType()
    {
        return type;
    }

    public Sprite GetIcon()
    {
        return icon;
    }
}
