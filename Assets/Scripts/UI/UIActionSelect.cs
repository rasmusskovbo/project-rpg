using System;
using Enemies;
using UnityEngine.UI;
using UnityEngine;

public class UIActionSelect : MonoBehaviour
{
    [SerializeField] private GameObject skillMenuUI; // can be fetched with FindObject
    [SerializeField] private Button attackButton;
    [SerializeField] private Button defendButton;
    [SerializeField] private Button supportButton;
    private CombatAction lastAction;
    private UICombatMoveSelect _uiCombatMoveSelect;

    private void Awake()
    {
        _uiCombatMoveSelect = FindObjectOfType<UICombatMoveSelect>();
    }

    public void OnAttackSelect()
    {
        lastAction = CombatAction.ATTACK;
        _uiCombatMoveSelect.InitiateCombatMoves(lastAction);
        ShowSkillSelect();
    }
    
    public void OnDefendSelect()
    {
        lastAction = CombatAction.DEFEND;
        _uiCombatMoveSelect.InitiateCombatMoves(lastAction);
        ShowSkillSelect();
    }
    
    public void OnSupportSelect()
    {
        lastAction = CombatAction.SUPPORT;
        _uiCombatMoveSelect.InitiateCombatMoves(lastAction);
        ShowSkillSelect();
    }

    private void ShowSkillSelect()
    {
        skillMenuUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetPrimaryButton()
    {
        if (lastAction == CombatAction.DEFEND)
        {
            defendButton.Select();
        } else if (lastAction == CombatAction.SUPPORT)
        {
            supportButton.Select();
        }
        else
        {
            attackButton.Select();
        }
    }
}
