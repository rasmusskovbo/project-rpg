using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpeedManager
{
    public SpeedManager(List<GameObject> incomingUnits)
    {
        activeUnits = new List<Unit>();
        
        foreach (GameObject go in incomingUnits)
        {
            Unit component = go.GetComponent<Unit>();
            activeUnits.Add(component);
        }
        
        PrintTurnOrder();
        Debug.Log(activeUnits.Count);
        activeUnits.Sort();
        PrintTurnOrder();
        
        SetCounter();
    }

    private List<Unit> activeUnits;
    private Unit nextUnitToAct;
    private float counter;

    public void SetCounter()
    {
        counter = 0;
        
        foreach (Unit unit in activeUnits)
        {
            if (unit.CurrentSpeed > counter)
            {
                counter = unit.CurrentSpeed;
            }
        }
    }

    public Unit GetNextTurn()
    {
        // Need to update active units after each turn
        // In case a unit died, remove it from list.
        // Seems like it doesnt add to the back of the list. Investigate
        foreach (Unit unit in activeUnits)
        {
            Debug.Log("Counter: " + counter);
            Debug.Log("Unit speed: " + unit.CurrentSpeed);
            if (unit.CurrentSpeed <= counter)
            {
                nextUnitToAct = unit;
                
                activeUnits.Remove(nextUnitToAct);
                counter--;
                Debug.Log("Next unit to act: " + nextUnitToAct.UnitName);
                break;
            }
            counter--;
            if (counter <= 0) SetCounter();
        }
        
        activeUnits.Add(nextUnitToAct);
        return nextUnitToAct;
    }

    public void PrintTurnOrder()
    {
        foreach (Unit unit in activeUnits)
        {
            Debug.Log("Unit order: " + unit.UnitName + ". Speed: " + unit.CurrentSpeed);
        }
    }
}
