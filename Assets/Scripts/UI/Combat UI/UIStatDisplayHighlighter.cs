using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIStatDisplayHighlighter : MonoBehaviour
{
    [SerializeField] private float highlightZoom = 1.2f;
    
    private CombatSystem combatSystem;
    private UIPlayerInputController inputController;
    private int referencedCount = -1;
    private List<UIStatDisplay> allEnemyStatDisplays;
    private List<UIStatDisplay> activeStatDisplays;

    private void Start()
    {
        combatSystem = FindObjectOfType<CombatSystem>();
        inputController = FindObjectOfType<UIPlayerInputController>();
        
        allEnemyStatDisplays = FindObjectsOfType<UIStatDisplay>().ToList();
        var playerStatDisplay = allEnemyStatDisplays.Find(statDisplay => statDisplay.ConnectedUnit.IsPlayerUnit);
        allEnemyStatDisplays.Remove(playerStatDisplay);
    }
    
    void Update()
    {
        if (combatSystem.State != CombatState.PLAYER_TARGET_SELECT) return;
        
        UpdateHighlights();
        UpdateInactiveDisplays();
    }

    // If the amount of enemies has not changed, do not use resources on updating list.
    private void UpdateHighlights()
    {
        if (inputController.MaxTargetIndex == referencedCount)
        {
            int indexOfEnemyToHighlight = inputController.TargetIndex;
            HighlightTargetedEnemy(indexOfEnemyToHighlight);
        }
        else
        {
            referencedCount = FindActiveStatDisplays();
        }
    }

    private void UpdateInactiveDisplays()
    {
        allEnemyStatDisplays.ForEach(statDisplay =>
        {
            if (statDisplay.ConnectedUnit.IsAlive) return;
            if (statDisplay.gameObject.GetComponentsInChildren<RectTransform>().Length < 4) return;
            
            statDisplay.gameObject.GetComponentsInChildren<RectTransform>()[3].gameObject.SetActive(false);
            statDisplay.gameObject.GetComponentInChildren<Image>().color = Color.grey;
            statDisplay.transform.localScale = new Vector3(1, 1);
        });
    }

    private int FindActiveStatDisplays()
    {
        activeStatDisplays = FindObjectsOfType<UIStatDisplay>().ToList().FindAll(statDisplay => statDisplay.ConnectedUnit.IsAlive);
        Debug.Log("Active stat displays: " + FindObjectsOfType<UIStatDisplay>());
        
        var playerStatDisplay = activeStatDisplays.Find(statDisplay => statDisplay.ConnectedUnit.IsPlayerUnit);
        activeStatDisplays.Remove(playerStatDisplay);

        activeStatDisplays.Reverse();
        return activeStatDisplays.Count - 1;
    }

    private void HighlightTargetedEnemy(int indexOfEnemyToHighlight)
    {
        Debug.Log("Active statdispl: " + activeStatDisplays.Count);
        for (int i = 0; i < activeStatDisplays.Count; i++)
        {
            if (i == indexOfEnemyToHighlight)
            {
                activeStatDisplays[i].transform.localScale = new Vector3(highlightZoom, highlightZoom);
            }
            else
            {
                activeStatDisplays[i].transform.localScale = new Vector3(1, 1);
            }
        }
    }

    public void ResetHighlights()
    {
        activeStatDisplays.ForEach(activeDisplay => activeDisplay.transform.localScale = new Vector3(1, 1));
    }
}
