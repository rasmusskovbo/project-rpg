using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class UIPlayerInputController : MonoBehaviour
{
    private CombatSystem combatSystem;
    private UICombatLog combatLog;
    
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
    private Vector2 defaultRectTransformOffset;
    
    [Header("Target Select")] 
    [SerializeField] private Transform cursor;
    [SerializeField] private Sprite cursorIdle;
    [SerializeField] private Sprite cursorOnSelect;
    [SerializeField] private float xCursorOffset;
    [SerializeField] private float yCursorOffset;
    [SerializeField] private float pointerClickAnimationTime = 1;
    private bool playerHasChosenATarget;
    
    // General selecting
    private Vector2 inputDirection;
    
    // Action selecting
    private Vector2 defaultActionCursorPosition;
    private List<GameObject> actionButtonsGameObjects; // used for targetting etc
    private List<GameObject> activeActionButtons; // used to disable action in SelectAction()
    private List<Vector2> selectableActionPositions;
    private CombatAction activeAction;
    private Vector2 lastActionPosition;
    private int actionIndex;
    private int maxActionIndex = 2;
    
    // Skill selecting
    private UISkillLoader skillLoader;
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
        skillLoader = FindObjectOfType<UISkillLoader>();
        combatLog = FindObjectOfType<UICombatLog>();
        
        InitiateActionSelectLists();

        selectableTargetPositions = new List<Vector2>();
    }
    
    void Start()
    {
        combatSystem = FindObjectOfType<CombatSystem>();
        
        SetActiveSelectPositions();
        InitiateSkillSelect();
        UpdateTargetablePositions();
        
        cursor.position = defaultActionCursorPosition;
        defaultSkillSelectorPosition = selector.position;
        lastSkillSelectorPosition = defaultSkillSelectorPosition;
    }
    
    private void InitiateActionSelectLists()
    {
        actionButtonsGameObjects = new List<GameObject>();
        selectableActionPositions = new List<Vector2>();
        activeActionButtons = new List<GameObject>();
        
        // Load default action buttons
        actionButtonsGameObjects.Add(attackAction);
        actionButtonsGameObjects.Add(defendAction);
        actionButtonsGameObjects.Add(supportAction);
        
        // Load default into active
        actionButtonsGameObjects.ForEach(buttonGO => activeActionButtons.Add(buttonGO));
    }
    
    /*
     * Set active positions for choosing action
     */
    private void SetActiveSelectPositions()
    {
        selectableActionPositions.Clear();

        foreach (GameObject gameObject in actionButtonsGameObjects)
        {
            Vector2 modifiedTransform = gameObject.transform.position;
            modifiedTransform.x += xActionCursorOffset;
            modifiedTransform.y += yActionCursorOffset;
            selectableActionPositions.Add(modifiedTransform);
        }

        defaultActionCursorPosition = selectableActionPositions[0];
        actionIndex = 0;
        maxActionIndex = selectableActionPositions.Count - 1;
    }

    public void ResetActionSelectUI()
    {
        activeActionButtons.Clear();
        actionButtonsGameObjects.ForEach(buttonGO =>
        {
            activeActionButtons.Add(buttonGO);
            buttonGO.GetComponent<Image>().color = Color.white;
        });
        
        ResetActionSelectPanels();
        MoveCursorToDefaultActionSelect();
    }

    public void DisableChosenAction(CombatAction action)
    {
        Debug.Log("Combat action chosen: " + action);
        GameObject actionButtonGO = activeActionButtons.Find(buttonGO =>
            buttonGO.name
                .ToLower()
                .Contains(
                    action.ToString().ToLower()
                )
        );

        activeActionButtons.Remove(actionButtonGO);
        actionButtonGO.gameObject.GetComponent<Image>().color = Color.black;
    }

    public bool IsActionDisabled(CombatAction action)
    {
        return !activeActionButtons.Find(buttonGO =>
            buttonGO.name
                .ToLower()
                .Contains(
                    action.ToString().ToLower()
                )
        );
    }

    private void InitiateSkillSelect()
    {
        defaultSkillSelectorPosition = selector.position;
        defaultRectTransformOffset = contentRectTransform.offsetMax;
        skillIndex = 0;
    }
    
    public void UpdateTargetablePositions()
    {
        selectableTargetPositions.Clear();

        List<GameObject> activeEnemies = combatSystem.GetActiveEnemies();
        if (activeEnemies.Count <= 0) return;

        activeEnemies.ForEach(enemyObject =>
        {
            Vector2 modifiedTransform = enemyObject.transform.position;
            modifiedTransform.x += xCursorOffset;
            modifiedTransform.y += yCursorOffset;
            selectableTargetPositions.Add(modifiedTransform);
        });
        
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
            StartCoroutine(AnimateCursorClick());
            playerHasChosenATarget = true;
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
            ResetActionSelectPanels();
        }
        
        if (combatSystem.State == CombatState.PLAYER_TARGET_SELECT)
        {
            if (playerHasChosenATarget) return;
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
        var transformPosition = selector.transform.position;
        
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
        
        if (inputDirection.Equals(Vector2.up) || inputDirection.Equals(Vector2.left))
        {
            if (targetIndex > 0)
            {
                cursor.position = selectableTargetPositions[targetIndex - 1];
                targetIndex--;
            }
            else
            {
                targetIndex = maxTargetIndex;
                cursor.position = selectableTargetPositions[maxTargetIndex];
            }
        }

        if (inputDirection.Equals(Vector2.down) || inputDirection.Equals(Vector2.right))
        {
            if (targetIndex < maxTargetIndex)
            {
                cursor.position = selectableTargetPositions[targetIndex + 1];
                targetIndex++;
            }
            else
            {
                targetIndex = 0;
                cursor.position = selectableTargetPositions[0];
            }
        }
    }
    
    /*
     * OnSubmit Functions
     * Rework to compare chosen action
     * with active actions instead.
     * Alternatively, do not disable or destroy anything
     * and simply grey it out (e.g skill select)
     */
    void SelectAction()
    {
        switch (actionIndex)
        {
            case 0:
                if (IsActionDisabled(CombatAction.ATTACK)) return;
                
                activeAction = CombatAction.ATTACK;
                attackPanel.GetComponent<Image>().sprite = selectedAttackBG;
                ZoomSelectedActionIcon(attackAction);
                break;
            case 1:
                if (IsActionDisabled(CombatAction.DEFEND)) return;
                
                activeAction = CombatAction.DEFEND;
                defendPanel.GetComponent<Image>().sprite = selectedDefendBG;
                ZoomSelectedActionIcon(defendAction);
                break;
            case 2:
                if (IsActionDisabled(CombatAction.SUPPORT)) return;
                
                activeAction = CombatAction.SUPPORT;
                supportPanel.GetComponent<Image>().sprite = selectedSupportBG;
                ZoomSelectedActionIcon(supportAction);
                break;
        }
        
        lastActionPosition = cursor.position;
        
        // Reset Skill Selector
        maxSkillIndex = skillLoader.InitiateCombatMoves(activeAction);
        selector.position = defaultSkillSelectorPosition; 
        selectorPosition = 0;
        skillIndex = 0;
        contentRectTransform.offsetMax = defaultRectTransformOffset;

        combatSystem.State = CombatState.PLAYER_SKILL_SELECT;
        
    }
    
    void SelectSkill()
    {
        CombatMove chosenSkill = skillLoader.GetSkill(skillIndex);
        
        if (chosenSkill.GetCooldownTracker().isMoveOnCooldown())
        {
            combatLog.MoveIsOnCooldown(chosenSkill);
            return;
        }
        
        // Save selector position
        // Set target cursor position to default or last selected target.
        lastSkillSelectorPosition = selector.position;
        
        cursor.position = defaultTargetCursorPosition; // Should be last selected target
        targetIndex = 0; // When above is last selected target, do not reset index.
        
        combatSystem.OnSkillSelect(chosenSkill);
    }
    
    void SelectTarget()
    {
        combatSystem.OnTargetSelect(targetIndex);
        ResetActionSelectPanels();
    }
    
    private void ZoomSelectedActionIcon(GameObject action)
    {
        action.GetComponent<RectTransform>().localScale = new Vector3(selectedActionScale, selectedActionScale);
    }

    private IEnumerator AnimateCursorClick()
    {
        cursor.GetComponentInChildren<SpriteRenderer>().sprite = cursorOnSelect;
        yield return new WaitForSeconds(pointerClickAnimationTime);
        cursor.GetComponentInChildren<SpriteRenderer>().sprite = cursorIdle;
    }

    void ResetActionSelectPanels()
    {
        attackPanel.GetComponent<Image>().sprite = defaultAttackBG;
        defendPanel.GetComponent<Image>().sprite = defaultDefendBG;
        supportPanel.GetComponent<Image>().sprite = defaultSupportBG;
        
        attackAction.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
        defendAction.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
        supportAction.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
    }

    public void MoveCursorToDefaultActionSelect()
    {
        actionIndex = 0;
        cursor.position = defaultActionCursorPosition;
    }

    public bool PlayerHasChosenATarget
    {
        get => playerHasChosenATarget;
        set => playerHasChosenATarget = value;
    }
}
