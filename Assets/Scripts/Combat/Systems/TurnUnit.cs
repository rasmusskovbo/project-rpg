using System;

public class TurnUnit : IComparable
{
    public TurnUnit(Unit unit, float turnRatio)
    {
        this.unit = unit;
        this.turnRatio = turnRatio;
        turnCounter = turnRatio;
    }

    private Unit unit;
    private float turnRatio;
    private float turnCounter;

    public Unit Unit
    {
        get => unit;
        set => unit = value;
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
