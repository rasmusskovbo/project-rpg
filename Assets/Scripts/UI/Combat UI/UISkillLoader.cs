using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

/*
 * Should ONLY be Responsible for loading skills to the skill UI - tracking active skills should be: 
 * The active skills for the character should be supplied from the player skill manager object
 * The active skills will be CombatMoves directly, and will include a variable or object indicating whether the skill is currently on cooldown
 */
public class UISkillLoader : MonoBehaviour
{

    [SerializeField] private GameObject skillItemPrefab; // The prefab for the list item (logo, name etc)
    [SerializeField] private RectTransform contentRectTransform; // Scroll window transform

    private SkillManager SkillManager;
    private List<CombatMove> combatMovesInUI;

    private void Awake()
    {
        SkillManager = FindObjectOfType<SkillManager>();
        combatMovesInUI = new List<CombatMove>();
    }

    /*
     * also check if the supplied combatmove is on a cooldown, if then make not selectable
     */
    public int InitiateCombatMoves(CombatAction action)
    {
        ClearSkillUI();
        combatMovesInUI.Clear();
        
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

        SkillManager.GetActiveCombatMoves().ForEach(combatMove =>
        {
            if (filters.Contains(combatMove.GetType()))
            {
                //Debug.Log("Move added: " + combatMove.GetName());
                AddMoveToUI(combatMove);
            }
        });

        return GetMaxIndex();

    }

    private void AddMoveToUI(CombatMove combatMove)
    {
        var item = Instantiate(skillItemPrefab);
        
        item.GetComponentsInChildren<Image>()[0].sprite = combatMove.getIconImage();
        item.GetComponentsInChildren<Image>()[1].sprite = combatMove.GetIcon();
        item.GetComponentsInChildren<TextMeshProUGUI>()[0].SetText(combatMove.GetName());
        item.GetComponentsInChildren<TextMeshProUGUI>()[1].SetText(combatMove.GetPower().ToString());
        item.GetComponentsInChildren<TextMeshProUGUI>()[2].SetText(combatMove.GetCooldown().ToString());
        
        item.transform.SetParent(contentRectTransform);
        item.transform.localScale = Vector2.one;

        combatMovesInUI.Add(combatMove);
        
        //Debug.Log("Is move on CD: " + combatMove.GetName() + ", " + combatMove.GetCooldownTracker().isMoveOnCooldown());
        
        if (combatMove.GetCooldownTracker().isMoveOnCooldown())
        {
            item.GetComponentsInChildren<Image>()[0].color = Color.black;
            item.GetComponentsInChildren<TextMeshProUGUI>()[2].color = Color.red;
            item.GetComponentsInChildren<TextMeshProUGUI>()[2].SetText(combatMove.GetCooldownTracker().GetRemainingCooldown().ToString() + "/" + combatMove.GetCooldown().ToString());
        }
        else
        {
            item.GetComponentsInChildren<Image>()[0].color = Color.white;
            item.GetComponentsInChildren<TextMeshProUGUI>()[2].color = Color.white;
        }
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
        return combatMovesInUI.Count > 0 ? combatMovesInUI.Count - 1 : 0;
    }

    public CombatMove GetSkill(int index)
    {
        return combatMovesInUI[index];
    }
}
    
    
   
