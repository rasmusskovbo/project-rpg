using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICombatLog : MonoBehaviour
{
    [SerializeField] private GameObject combatLoGameObject;
    private List<TextMeshProUGUI> combatLog;

    private void Awake()
    {
        combatLog = new List<TextMeshProUGUI>();
        
        TextMeshProUGUI[] textLineItems = combatLoGameObject.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (var textMeshProUGUI in textLineItems)
        {
            combatLog.Add(textMeshProUGUI);
        }
        
    }
    
    /*
     * Printing to the combatlog and moving the lines in the UI.
     * Functionality methods
     */
    public void PrintToLog(string msg)
    {
        for (int i = 0; i < combatLog.Count - 1; i++)
        {
            combatLog[i].text = combatLog[i + 1].text;
        }

        combatLog[combatLog.Count - 1].text = msg;
    }
    
    public void Clear()
    {
        combatLog.ForEach(textObject => textObject.text = "");
    }
    
    /*
     * Event specific text:
     */
    public void StartOfCombat()
    {
        string line = "Enemies have appeared. Speed determines turn order.";
        PrintToLog(line);
    }

    public void NextPlayerAction(int remainingActions)
    {
        string line = "";
        if (remainingActions > 1)
        {
            line = "You have " + remainingActions + " remaining actions left.";
        }
        else
        {
            line = "You have another action left.";
        }

        PrintToLog(line);
    }

    public void PlayerTurn()
    {
        string line = "Player's turn! Choose an action.";
        PrintToLog(line);
    }

    public void PlayerUsedCombatMove(CombatMove move, Unit target, float damage)
    {
        string line = "You used " + move.GetName() +
                      " on " + target.UnitName +
                      ". It hit for " + damage + " " + move.GetType() + " damage.";
        PrintToLog(line);
    }

    public void MoveIsOnCooldown(CombatMove move)
    {
        string line = move.GetName() + " is currently on cooldown.";
        PrintToLog(line);
    }


}
