using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject skillSelectUI;
    [SerializeField] private GameObject actionSelectUI;
    [SerializeField] private GameObject targetSelectUI;
    private CombatAction activeAction;
    private CombatSystem _combatSystem;

    private void Awake()
    {
        _combatSystem = FindObjectOfType<CombatSystem>();
    }

    private void Update()
    {
        switch (_combatSystem.State)
        {
            case CombatState.PLAYER_ACTION_SELECT:
                actionSelectUI.SetActive(true);
                skillSelectUI.SetActive(false);
                targetSelectUI.SetActive(false);
                break;
            case CombatState.PLAYER_SKILL_SELECT:
                actionSelectUI.SetActive(false);
                skillSelectUI.SetActive(true);
                targetSelectUI.SetActive(false);
                break;
            case CombatState.PLAYER_TARGET_SELECT:
                actionSelectUI.SetActive(false);
                skillSelectUI.SetActive(false);
                targetSelectUI.SetActive(true);
                break;
            default:
                actionSelectUI.SetActive(false);
                skillSelectUI.SetActive(false);
                targetSelectUI.SetActive(false);
                break;
        }
    }
}
