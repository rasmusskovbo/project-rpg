using UnityEngine;

public class CombatMove
{
    private CombatMoveBase _base;
    private CooldownTracker cooldownTracker;

    public CombatMove(CombatMoveBase mBase)
    {
        _base = mBase;
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

    public int GetDuration()
    {
        return _base.Duration;
    }

    public CombatMoveTargets GetTargets()
    {
        return _base.Targets;
    }

    public CombatEffectType GetEffectType()
    {
        return _base.EffectType;
    }

    public bool GetExpiresAtStartOfTurn()
    {
        return _base.ExpiresAtStartOfTurn;
    }
}
