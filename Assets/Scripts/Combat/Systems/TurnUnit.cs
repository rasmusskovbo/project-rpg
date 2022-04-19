using System;

public class TurnUnit : IComparable
{
    public TurnUnit(CombatUnit combatUnit, float turnRatio)
    {
        this._combatUnit = combatUnit;
        this.turnRatio = turnRatio;
        turnCounter = turnRatio;
    }

    private CombatUnit _combatUnit;
    private float turnRatio;
    private float turnCounter;

    public CombatUnit CombatUnit
    {
        get => _combatUnit;
        set => _combatUnit = value;
    }

    public float TurnRatio
    {
        get => turnRatio;
        set => turnRatio = value;
    }

    public float TurnCounter
    {
        get => turnCounter;
        set => turnCounter = value;
    }
    
    public int CompareTo(object obj)
    {
        TurnUnit other = obj as TurnUnit;
        return turnCounter.CompareTo(other.turnCounter);
    }
}
