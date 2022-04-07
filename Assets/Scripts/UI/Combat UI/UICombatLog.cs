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

    public void PrintAttackMove(CombatMove move, Unit target, float damage)
    {
        string line = "You used " + move.GetName() +
                      " on " + target +
                      ". It hit for " + damage + " " + move.GetType() + " damage.";
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
