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

    public void EnemyTurn(CombatUnit enemy)
    {
        string line = enemy.UnitName + "'s turn to act:";
        PrintToLog(line);
    }

    public void PlayerWon()
    {
        string line = "All enemies defeated. You won!";
        PrintToLog(line);
    }

    public void PlayerHealed(CombatMove move)
    {
        string line = "Your " + move.GetName() + " healed for " + move.GetPower();
        PrintToLog(line);
    }

    public void PlayerAppliedBlock(CombatMove move)
    {
        string line = "You gained " + move.GetPower() + " physical block!";
        PrintToLog(line);
    }

    public void UnitAppliedEffect(CombatUnit unit, CombatMove move)
    {
        string line = unit.UnitName + " gained " + move.GetName() + ".";
        PrintToLog(line);
    }

    public void UsedOffensiveCombatMove(CombatMove move, CombatUnit attacker, CombatUnit target, float damage)
    {
        string line = attacker.UnitName + " used " + move.GetName() +
                      " on " + target.UnitName;
        PrintToLog(line);
        line = ">> It hit for " + damage + " " + move.GetType() + " damage.";
        PrintToLog(line);
    }

    public void DamagePlayer(CombatUnit enemy, CombatMove move, float incomingDamage)
    {
        string line = enemy.UnitName + " used " + move.GetName() +
                      ". It hit for " + incomingDamage + " " + move.GetType() + " damage.";
        PrintToLog(line);
    }
    public void MoveIsOnCooldown(CombatMove move)
    {
        string line = move.GetName() + " is currently on cooldown.";
        PrintToLog(line);
    }

    public void UnitIsSilenced(CombatMove move, CombatUnit unit)
    {
        string line = move.GetName() + " is currently on cooldown.";
        PrintToLog(line);
    }


}
