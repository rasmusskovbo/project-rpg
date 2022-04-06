using System.Collections.Generic;
using System.Numerics;
using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class UICombatMoveSelect : MonoBehaviour
{
    [Header("Dynamic Skills")]
    [SerializeField] private GameObject skillItemPrefab; // The prefab for the list item (logo, name etc)
    [SerializeField] private List<CombatMoveBase> combatMoveBases; // A comprehensive list of all combat moves in the game

    // Section for managing UI element
    [Header("Action Navigation")] 
    [SerializeField] private UIActionSelect actionSelectUI;
    [SerializeField] private GameObject skillMenuUI;
    private CombatSystem _combatSystem;
    private List<CombatMove> activeCombatMoves;
    
    // Assets for selecting skill with scrollview.
    [Header("Selector")] 
    [SerializeField] private float selectorMoveDistance;
    [SerializeField] private RectTransform selector;
    [SerializeField] private float scrollOffset;
    [SerializeField] private RectTransform rectTransform;
    private Vector2 defaultSelectorPosition;
    private int selectorPosition;
    private Vector2 inputDirection;
    private int index;
    private int maxIndex;
    private int selectorScrollCap = 3;
    
    private void Awake()
    {
        _combatSystem = FindObjectOfType<CombatSystem>();
        activeCombatMoves = new List<CombatMove>();
        defaultSelectorPosition = selector.position;
    }

    public void InitiateCombatMoves(CombatAction action)
    {
        activeCombatMoves.Clear();
        ClearSkillUI();
        selector.position = defaultSelectorPosition;
        selectorPosition = 0;
        index = 0;
        
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
        
        SetMaxIndex();
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
        item.transform.SetParent(rectTransform);
        item.transform.localScale = Vector2.one;
    }

    private void ClearSkillUI()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetMaxIndex()
    {
        if (activeCombatMoves.Count > 0)
        {
            maxIndex = activeCombatMoves.Count - 1;
        }
        else
        {
            maxIndex = 0;
        }
    }

    void OnSubmit()
    {
        // flash select animation
        // Send scriptable object of skill to combat system.
        _combatSystem.OnSkillSelect(activeCombatMoves[index]);
    }

    void OnCancel()
    {
        actionSelectUI.gameObject.SetActive(true);
        actionSelectUI.SetPrimaryButton();
        skillMenuUI.SetActive(false);
    }
    
    // Animate selector transitions
    void OnMoveSelector(InputValue value)
    {
        inputDirection = value.Get<Vector2>();
        var transformPosition = selector.transform.position;

        /*
        Debug.Log("Current index: " + index);
        Debug.Log("Scrollcap: " + indexScrollCap);
        Debug.Log("Selector position: " + selectorPosition);
        */
        
        if (inputDirection.Equals(Vector2.up))
        {
            if (index > 0)
            {
                if (selectorPosition > 0)
                {
                    transformPosition.y += selectorMoveDistance;
                    selectorPosition--;
                }
                else if (selectorPosition == 0)
                {
                    this.rectTransform.offsetMax -= new Vector2(0, +scrollOffset);
                }
                
                index--;
            }
        }
        
        if (inputDirection.Equals(Vector2.down))
        {
            Debug.Log("Index & max index: " + index + ", " + maxIndex);
            if (index < maxIndex)
            {
                if (selectorPosition < selectorScrollCap)
                {
                    transformPosition.y -= selectorMoveDistance;
                    selectorPosition++;
                }
                else if (selectorPosition == selectorScrollCap)
                {
                    this.rectTransform.offsetMax -= new Vector2(0, -scrollOffset);
                }
                
                index++;
            }
            
        }
        
        selector.position = transformPosition;
        
    }

}
    
    
   
