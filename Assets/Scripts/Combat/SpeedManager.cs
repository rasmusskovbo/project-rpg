using System;
using System.Collections.Generic;
using UnityEngine;

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
        return this.turnCounter.CompareTo(other.turnCounter);
        //return Mathf.RoundToInt(this.CurrentSpeed - other.CurrentSpeed);
    }
}

public class SpeedManager
{
    /*
     * 1) Get all active units
     * 2) Find the fastest (sort)
     * 3) Fastest is the baseline (will first every round)
     * 4) Use ratio (fastest speed/slower unit speed)
     * 5) If unit's turn counter < current turn + 1 -> act and increment (unit's turn counter + unit's turn ratio)
     * 6) Else, do not increment and check again next round.
     */
    public SpeedManager(List<GameObject> incomingUnits)
    {
        sortedUnits = new List<Unit>();
        activeUnits = new List<TurnUnit>();
        
        incomingUnits.ForEach(go => sortedUnits.Add(go.GetComponent<Unit>()));
        sortedUnits.Sort();
        
        SetupTurnList();
    }

    private List<Unit> sortedUnits;
    
    private List<TurnUnit> activeUnits;
    private Unit nextUnitToAct;
    
    private float fastestSpeed;
    private int currentTurn = 1;

    public void SetupTurnList()
    {
        fastestSpeed = sortedUnits[0].CurrentSpeed;

        sortedUnits.ForEach(unit =>
        {
            activeUnits.Add(GetTurnUnit(unit));
        });
        
        activeUnits.Sort();
        
        PrintTurnOrder();
    }

    public TurnUnit GetTurnUnit(Unit unit)
    {
        float turnRatio = fastestSpeed / unit.CurrentSpeed;

        return new TurnUnit(
            unit,
            turnRatio
        );
    }
    
    public Unit GetNextTurn()
    {
        List<TurnUnit> unitsToActThisTurn = new List<TurnUnit>();
        
        // Check units turn counter for turn
        activeUnits.ForEach(unit =>
        {
            if (unit.TurnCounter < currentTurn + 1)
            {
                unitsToActThisTurn.Add(unit);
            }

        });

        // If no units are left to act this turn then increase turn counter
        // and call function again to return first from next turn
        if (unitsToActThisTurn.Count > 0)
        {
            // Make sure the lowest unit with turn counter starts this turn
            unitsToActThisTurn.Sort();
        
            // Cache reference
            TurnUnit nextUnitToActThisTurn = unitsToActThisTurn[0];
        
            // Increase the turn counter of the unit that's about to act
            activeUnits.Find(turnUnit => turnUnit.Equals(nextUnitToActThisTurn)).TurnCounter +=
                nextUnitToActThisTurn.TurnRatio;  
        
            // Return the unit to combat system.
        
            return unitsToActThisTurn[0].Unit;
        }
        else
        {
            currentTurn++;
            return GetNextTurn();
        }
        
    }

    public void PrintTurnOrder()
    {
        activeUnits.ForEach(unit => Debug.Log("Name: " + unit.Unit.UnitName + " -- Speed: " + unit.Unit.CurrentSpeed));
    }
}
