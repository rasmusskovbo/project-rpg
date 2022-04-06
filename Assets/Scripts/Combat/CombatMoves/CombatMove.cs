using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.UI;

public class CombatMove
{
    private CombatMoveBase _base;
    private int level;

    public CombatMove(CombatMoveBase mBase, int mLevel)
    {
        _base = mBase;
        level = mLevel;
    }

    public Sprite getIconImage()
    {
        return _base.IconImage;
    }
    
    public string GetName()
    {
        return _base.name;
    }

    public int GetPower()
    {
        return _base.Power;
    }

    public int GetCooldown()
    {
        return _base.Cooldown;
    }

    public CombatMoveType GetType()
    {
        return _base.Type.GetType();
    }

    public Sprite GetIcon()
    {
        return _base.Type.GetIcon();
    }
    
}
