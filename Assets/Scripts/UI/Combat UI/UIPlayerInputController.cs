using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class UIPlayerInputController : MonoBehaviour
{
    private CombatSystem combatSystem;
    
    // TODO UI highlighting should be refactored to Ui Animation Controller
    [Header("Action Select")]
    [SerializeField] private GameObject attackAction;
    [SerializeField] private GameObject defendAction;
    [SerializeField] private GameObject supportAction;
    [SerializeField] private GameObject attackPanel;
    [SerializeField] private GameObject defendPanel;
    [SerializeField] private GameObject supportPanel;
    [SerializeField] private Sprite defaultAttackBG;
    [SerializeField] private Sprite selectedAttackBG;
    [SerializeField] private Sprite defaultDefendBG;
    [SerializeField] private Sprite selectedDefendBG;
    [SerializeField] private Sprite defaultSupportBG;
    [SerializeField] private Sprite selectedSupportBG;
    [SerializeField] private float xActionCursorOffset;
    [SerializeField] private float yActionCursorOffset;
    [SerializeField] private float selectedActionScale = 1;
    
    [Header("Skill Select")] 
    [SerializeField] private float selectorMoveDistance;
    [SerializeField] private RectTransform selector;
    [SerializeField] private float scrollOffset;
    [SerializeField] private RectTransform contentRectTransform;
    
    [Header("Target Select")] 
    [SerializeField] private Transform cursor;
    [SerializeField] private Sprite cursorIdle;
    [SerializeField] private Sprite cursorOnSelect;
    [SerializeField] private float xCursorOffset;
    [SerializeField] private float yCursorOffset;
    
    // General selecting
    private Vector2 inputDirection;
    
    // Action selecting
    private Vector2 defaultActionCursorPosition;
    private List<GameObject> actionButtons;
    private List<Vector2> selectableActionPositions;
    private CombatAction activeAction;
    private Vector2 lastActionPosition;
    private int actionIndex;
    private int maxActionIndex = 2;
    
    // Skill selecting
    private UISkillLoader _skillLoader;
    private Vector2 defaultSkillSelectorPosition;
    private Vector2 lastSkillSelectorPosition;
    private int selectorPosition;
    private int selectorScrollCap = 3;
    private int skillIndex;
    private int maxSkillIndex;
    
    // Target selecting
    private Vector2 defaultTargetCursorPosition;
    private int targetIndex;
    private int maxTargetIndex;
    private List<Vector2> selectableTargetPositions;

    void Awake()
    {
        _skillLoader = FindObjectOfType<UISkillLoader>();
        
        // Action list
        // TODO When implementing multiple skills per turn, this needs to be updated
        InitiateActionSelectLists();
        selectableTargetPositions = new List<Vector2>();
    }
    
    void Start()
    {
        combatSystem = FindObjectOfType<CombatSystem>();
        
        InitiateActionSelectPositions();
        InitiateSkillSelect();
        InitiateTargetSelectPositions();
        
        cursor.position = defaultActionCursorPosition;
        defaultSkillSelectorPosition = selector.position;
        lastSkillSelectorPosition = defaultSkillSelectorPosition;
    }
    
    private void InitiateActionSelectLists()
    {
        actionButtons = new List<GameObject>();
        actionButtons.Add(attackAction);
        actionButtons.Add(defendAction);
        actionButtons.Add(supportAction);

        selectableActionPositions = new List<Vector2>();
    }
    
    /*
     * Initiating cursor positions
     */
    private void InitiateActionSelectPositions()
    {
        foreach (GameObject gameObject in actionButtons)
        {
            Vector2 modifiedTransform = gameObject.transform.position;
            modifiedTransform.x += xActionCursorOffset;
            modifiedTransform.y += yActionCursorOffset;
            selectableActionPositions.Add(modifiedTransform);
        }

        defaultActionCursorPosition = selectableActionPositions[0];
        actionIndex = 0;
    }

    private void InitiateSkillSelect()
    {
        maxSkillIndex = _skillLoader.GetMaxIndex();
        defaultSkillSelectorPosition = selector.position;
    }
    
    private void InitiateTargetSelectPositions()
    {
        foreach (Transform transform in combatSystem.GetActiveEnemyStations())
        {
            Vector2 modifiedTransform = transform.position;
            modifiedTransform.x += xCursorOffset;
            modifiedTransform.y += yCursorOffset;
            selectableTargetPositions.Add(modifiedTransform);
        }

        combatSystem.GetActiveEnemyStations();
        defaultTargetCursorPosition = selectableTargetPositions[0];
        targetIndex = 0;
        maxTargetIndex = selectableTargetPositions.Count - 1;
    }

    /*
     *  On movement input from the user, filter from state
     *  and move correct UI element.
     */
    void OnMoveSelector(InputValue value)
    {
        inputDirection = value.Get<Vector2>();
        
        if (combatSystem.State == CombatState.PLAYER_ACTION_SELECT)
        {
            MoveActionCursor();
        }
        
        if (combatSystem.State == CombatState.PLAYER_SKILL_SELECT)
        {
            MoveSkillSelector();
        }
        
        if (combatSystem.State == CombatState.PLAYER_TARGET_SELECT)
        {
            MoveTargetCursor();
        }
        
    }
    
    /*
     *  On confirm or cancel input from the user, submit
     *  or go back to previous UI element.
     */

    void OnSubmit()
    {
        if (combatSystem.State == CombatState.PLAYER_ACTION_SELECT)
        {
            SelectAction();
        } else if (combatSystem.State == CombatState.PLAYER_SKILL_SELECT)
        {
            SelectSkill();
        } else if (combatSystem.State == CombatState.PLAYER_TARGET_SELECT)
        {
            SelectTarget();
        }
    }

    /*
     *  On Cancel from user.
     */
    void OnCancel()
    {
        if (combatSystem.State == CombatState.PLAYER_ACTION_SELECT)
        {
            // Open menu or do nothing
        }
        
        if (combatSystem.State == CombatState.PLAYER_SKILL_SELECT)
        {
            combatSystem.State = CombatState.PLAYER_ACTION_SELECT;
            cursor.position = lastActionPosition;
            ResetPanelBackgrounds();
        }
        
        if (combatSystem.State == CombatState.PLAYER_TARGET_SELECT)
        {
            combatSystem.State = CombatState.PLAYER_SKILL_SELECT;
            selector.position = lastSkillSelectorPosition;
        }
    }

    /*
     * Cursor movement Functions
     */
    private void MoveActionCursor()
    {

        if (inputDirection.Equals(Vector2.right))
        {
            if (actionIndex < maxActionIndex)
            {
                cursor.position = selectableActionPositions[actionIndex + 1];
                actionIndex++;
            }
            else
            {
                actionIndex = 0;
                cursor.position = selectableActionPositions[0];
            }
        }

        if (inputDirection.Equals(Vector2.left))
        {
            if (actionIndex > 0)
            {
                cursor.position = selectableActionPositions[actionIndex - 1];
                actionIndex--;
            }
            else
            {
                actionIndex = maxActionIndex;
                cursor.position = selectableActionPositions[maxActionIndex];
            }
        }
    }

    private void MoveSkillSelector()
    {
        Debug.Log("Inside skill selector");
        var transformPosition = selector.transform.position;

        
        Debug.Log("Current index: " + skillIndex);
        Debug.Log("Max index: " + maxSkillIndex);
        Debug.Log("Scrollcap: " + selectorScrollCap);
        Debug.Log("Selector position: " + selectorPosition);
        
        
        if (inputDirection.Equals(Vector2.up))
        {
            if (skillIndex > 0)
            {
                if (selectorPosition > 0)
                {
                    transformPosition.y += selectorMoveDistance;
                    selectorPosition--;
                }
                else if (selectorPosition == 0)
                {
                    this.contentRectTransform.offsetMax -= new Vector2(0, +scrollOffset);
                }
                
                skillIndex--;
            }
        }
        
        if (inputDirection.Equals(Vector2.down))
        {
            if (skillIndex < maxSkillIndex)
            {
                if (selectorPosition < selectorScrollCap)
                {
                    transformPosition.y -= selectorMoveDistance;
                    selectorPosition++;
                }
                else if (selectorPosition == selectorScrollCap)
                {
                    this.contentRectTransform.offsetMax -= new Vector2(0, -scrollOffset);
                }
                
                skillIndex++;
            }
            
        }
        
        selector.position = transformPosition;
    }

    private void MoveTargetCursor()
    {
        // If only one enemy, do not move cursor
        if (maxTargetIndex < 1) return;

        if (inputDirection.Equals(Vector2.up))
        {
            if (targetIndex > 0)
            {
                cursor.position = selectableTargetPositions[targetIndex - 1];
                targetIndex--;
            }
        }

        if (inputDirection.Equals(Vector2.down))
        {
            if (targetIndex < maxTargetIndex)
            {
                cursor.position = selectableTargetPositions[targetIndex + 1];
                targetIndex++;
            }
        }

        if (inputDirection.Equals(Vector2.right))
        {
            // If less than 4 monsters, disable controls.
            // If on the rightmost places in a group of 4, disable controls
            // If on the rightmost places in a group of 5, disable controls
            if (maxTargetIndex < 3) return;
            if (maxTargetIndex == 3 && targetIndex >= 2) return;
            if (maxTargetIndex == 4 && targetIndex >= 3) return;

            if ((maxTargetIndex == 4 && targetIndex == 2) || maxTargetIndex == 3)
            {
                cursor.position = selectableTargetPositions[targetIndex + 2];
                targetIndex += 2;
            }
            else if (maxTargetIndex == 4)
            {
                cursor.position = selectableTargetPositions[targetIndex + 3];
                targetIndex += 3;
            }
        }

        if (inputDirection.Equals(Vector2.left))
        {
            // If less than 4 monsters, disable controls.
            // If on the leftmost places in a group of 4, disable controls
            // If on the leftmost places in a group of 5, disable controls
            if (maxTargetIndex < 3) return;
            if (maxTargetIndex == 3 && targetIndex <= 1) return;
            if (maxTargetIndex == 4 && targetIndex <= 2) return;

            if (maxTargetIndex == 3)
            {
                cursor.position = selectableTargetPositions[targetIndex - 2];
                targetIndex -= 2;
            }
            else if (maxTargetIndex == 4)
            {
                cursor.position = selectableTargetPositions[targetIndex - 3];
                targetIndex -= 3;
            }
        }
    }
    
    /*
     * OnSubmit Functions
     */
    void SelectAction()
    {
        switch (actionIndex)
        {
            case 0:
                activeAction = CombatAction.ATTACK;
                attackPanel.GetComponent<Image>().sprite = selectedAttackBG;
                ZoomSelectedActionIcon(attackAction);
                break;
            case 1:
                activeAction = CombatAction.DEFEND;
                defendPanel.GetComponent<Image>().sprite = selectedDefendBG;
                ZoomSelectedActionIcon(defendAction);
                break;
            case 2:
                activeAction = CombatAction.SUPPORT;
                supportPanel.GetComponent<Image>().sprite = selectedSupportBG;
                ZoomSelectedActionIcon(supportAction);
                break;
        }
        
        lastActionPosition = cursor.position;
        
        // Reset Skill Selector
        _skillLoader.InitiateCombatMoves(activeAction);
        selector.position = defaultSkillSelectorPosition; 
        selectorPosition = 0;
        skillIndex = 0;
        maxSkillIndex = _skillLoader.GetMaxIndex();
        
        combatSystem.State = CombatState.PLAYER_SKILL_SELECT;
        
    }

    private void ZoomSelectedActionIcon(GameObject action)
    {
        action.GetComponent<RectTransform>().localScale = new Vector3(selectedActionScale, selectedActionScale);
    }

    void SelectSkill()
    {
        // Save selector position
        // Set target cursor position to default or last selected target.
        lastSkillSelectorPosition = selector.position;
        
        cursor.position = defaultTargetCursorPosition; // Should be last selected target
        targetIndex = 0; // When above is last selected target, do not reset index.

        combatSystem.OnSkillSelect(_skillLoader.GetSkill(skillIndex));
    }
    
    void SelectTarget()
    {
        
    }

    void ResetPanelBackgrounds()
    {
        attackPanel.GetComponent<Image>().sprite = defaultAttackBG;
        defendPanel.GetComponent<Image>().sprite = defaultDefendBG;
        supportPanel.GetComponent<Image>().sprite = defaultSupportBG;
        
        attackAction.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
        defendAction.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
        supportAction.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
    }
}
