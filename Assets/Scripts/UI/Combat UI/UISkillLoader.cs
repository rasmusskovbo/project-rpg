using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class UISkillLoader : MonoBehaviour
{

    [SerializeField] private GameObject skillItemPrefab; // The prefab for the list item (logo, name etc)
    [SerializeField] private List<CombatMoveBase> combatMoveBases; // A comprehensive list of all combat moves in the game
    [SerializeField] private RectTransform contentRectTransform;
    private List<CombatMove> activeCombatMoves;
    
    private void Awake()
    {
        activeCombatMoves = new List<CombatMove>();
    }

    public void InitiateCombatMoves(CombatAction action)
    {
        activeCombatMoves.Clear();
        ClearSkillUI();
        
        List<CombatMoveType> filters = new List<CombatMoveType>();
        
        if (action == CombatAction.ATTACK)
        {
            filters.Add(CombatMoveType.Physical);
            filters.Add(CombatMoveType.Magical);
        }
        else if (action == CombatAction.DEFEND)
        {
            filters.Add(CombatMoveType.Defend);
            filters.Add(CombatMoveType.Mitigate);
        }
        else if (action == CombatAction.SUPPORT)
        {
            filters.Add(CombatMoveType.Heal);
            filters.Add(CombatMoveType.Buff);
            filters.Add(CombatMoveType.Debuff);
        }

        foreach (var moveBase in combatMoveBases)
        {
            if (filters.Contains(moveBase.Type.GetType()))
            {
                AddMoveToUI(moveBase);
            }
        }

    }

    private void AddMoveToUI(CombatMoveBase moveBase)
    {
        var item = Instantiate(skillItemPrefab);
        CombatMove combatMove = new CombatMove(moveBase, 1); // TODO DYNAMIC lvl for scaling

        item.GetComponentsInChildren<Image>()[0].sprite = combatMove.getIconImage();
        item.GetComponentsInChildren<Image>()[1].sprite = combatMove.GetIcon();
        item.GetComponentsInChildren<TextMeshProUGUI>()[0].SetText(combatMove.GetName());
        item.GetComponentsInChildren<TextMeshProUGUI>()[1].SetText(combatMove.GetPower().ToString());
        item.GetComponentsInChildren<TextMeshProUGUI>()[2].SetText(combatMove.GetCooldown().ToString());

        activeCombatMoves.Add(combatMove);
        item.transform.SetParent(contentRectTransform);
        item.transform.localScale = Vector2.one;
    }

    public void ClearSkillUI()
    {
        foreach (Transform child in contentRectTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public int GetMaxIndex()
    {
        return activeCombatMoves.Count > 0 ? activeCombatMoves.Count - 1 : 0;
    }

    public CombatMove GetSkill(int index)
    {
        return activeCombatMoves[index];
    }
}
    
    
   
