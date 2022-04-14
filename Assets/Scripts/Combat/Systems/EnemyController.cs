using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private CombatMove attackMove;

    public CombatMove UseSkill()
    {
        return attackMove;
    }
    
}

