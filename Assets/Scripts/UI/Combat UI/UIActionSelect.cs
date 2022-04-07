using UnityEngine.UI;
using UnityEngine;

public class UIActionSelect : MonoBehaviour
{
    [SerializeField] private Button attackButton;
    [SerializeField] private Button defendButton;
    [SerializeField] private Button supportButton;
    private CombatAction lastAction;
    private UISkillSelect _uiSkillSelect;
    private CombatSystem _combatSystem;

    private void Awake()
    {
        _uiSkillSelect = FindObjectOfType<UISkillSelect>();
        _combatSystem = FindObjectOfType<CombatSystem>();
    }

    public void OnAttackSelect()
    {
        lastAction = CombatAction.ATTACK;
        _uiSkillSelect.InitiateCombatMoves(lastAction);
        ShowSkillSelect();
    }
    
    public void OnDefendSelect()
    {
        lastAction = CombatAction.DEFEND;
        _uiSkillSelect.InitiateCombatMoves(lastAction);
        ShowSkillSelect();
    }
    
    public void OnSupportSelect()
    {
        lastAction = CombatAction.SUPPORT;
        _uiSkillSelect.InitiateCombatMoves(lastAction);
        ShowSkillSelect();
    }

    private void ShowSkillSelect()
    {
        _combatSystem.State = CombatState.PLAYER_SKILL_SELECT;
        _uiSkillSelect.SetIsSelectingSkill(true);
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
