using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICombatLog : MonoBehaviour
{
    private List<TextMeshProUGUI> combatLog;

    private void Awake()
    {
        combatLog = new List<TextMeshProUGUI>();
        
        TextMeshProUGUI[] text = GetComponentsInChildren<TextMeshProUGUI>();

        foreach (var textMeshProUGUI in text)
        {
            combatLog.Add(textMeshProUGUI);
        }

        Debug.Log(combatLog[0].text + " index 0");
    }

    // TODO Should only provide types and then combatlog will format.
    // e.g. enemy name, skill, damage, type etc
    // This print function is for testing, however moving the log up works.
    public void PrintToLog(string msg)
    {
        for (int i = 0; i < combatLog.Count - 1; i++)
        {
            combatLog[i].text = combatLog[i + 1].text;
        }

        combatLog[combatLog.Count - 1].text = msg;
    }

    public void PrintAttackMove(CombatMove move, Enemy target, float damage, string type)
    {
        string line = "You used " + move.GetName() +
                      " on " + target.Name +
                      ". It hit for " + damage + " " + type + " damage.";
        PrintToLog(line);
    }

    public void Clear()
    {
        foreach (var textMeshProUGUI in combatLog)
        {
            textMeshProUGUI.text = "";
        }
    }
}
