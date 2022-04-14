using System.Collections.Generic;
using UnityEngine;

public class CombatMove
{
    private CombatMoveBase _base;
    private int level;
    private CooldownTracker cooldownTracker;

    public CombatMove(CombatMoveBase mBase, int mLevel)
    {
        _base = mBase;
        level = mLevel;
        cooldownTracker = new CooldownTracker(0);
    }

    public CooldownTracker GetCooldownTracker()
    {
        return cooldownTracker;
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

    public CombatAction GetActionType()
    {
        return _base.ActionType;
    }

    public Sprite GetIcon()
    {
        return _base.Type.GetIcon();
    }
    
}
