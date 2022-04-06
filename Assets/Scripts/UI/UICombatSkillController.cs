using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UICombatSkillController : MonoBehaviour
{
    [Header("Dynamic Skills")]
    [SerializeField] private GameObject skillItemPrefab;
    [SerializeField] private CombatMoveBase baseTest;
    [SerializeField] private List<CombatMoveBase> combatMoveBases;
    private CombatMove combatMoveTestComponent; // This should be a comprehensive list to choose from at runtime to map dynamically.
    [SerializeField] private int amount = 6;
    
    [Header("Action Navigation")] 
    [SerializeField] private UIActionSelect actionSelectUI;
    [SerializeField] private GameObject skillMenuUI;
    private CombatSystem _combatSystem;
    private List<CombatMove> activeCombatMoves;
    
    [Header("Selector")] 
    [SerializeField] private float selectorMoveDistance;
    [SerializeField] private RectTransform selector;
    [SerializeField] private float scrollOffset;
    [SerializeField] private RectTransform rectTransform;
    private int selectorPosition;
    private Vector2 inputDirection;
    private int index;
    private int maxIndex = 6;
    private int indexScrollCap = 3;
    
    private void Awake()
    {
        activeCombatMoves = new List<CombatMove>();
        _combatSystem = FindObjectOfType<CombatSystem>();
    }

    private void Start()
    {
        // Load all combat moves from available scriptable objects.
        foreach (var moveBase in combatMoveBases)
        {
            var item = Instantiate(skillItemPrefab);
            CombatMove combatMove = new CombatMove(moveBase, 1); // TODO DYNAMIC lvl for scaling
            
            item.GetComponentsInChildren<Image>()[0].sprite = combatMove.getIconImage();
            item.GetComponentsInChildren<Image>()[1].sprite = combatMove.getIconImage();
            item.GetComponentsInChildren<TextMeshProUGUI>()[0].SetText(combatMove.GetName());
            item.GetComponentsInChildren<TextMeshProUGUI>()[1].SetText(combatMove.GetPower().ToString());
            item.GetComponentsInChildren<TextMeshProUGUI>()[2].SetText(combatMove.GetCooldown().ToString());
            
            activeCombatMoves.Add(combatMove);
            item.transform.SetParent(rectTransform);
            item.transform.localScale = Vector2.one;
        }
        
        /* Instantiating of dynamic list items (test):
        combatMoveTestComponent = new CombatMove(baseTest, 1);
        rectTransform.GetComponent<RectTransform>();
        for (int i = 0; i < amount; i++)
        {
            // This is just one combat move being instantiated. This would be a set of specific components depending on whats available to the player.
            var item = Instantiate(skillItemPrefab);
            item.GetComponentsInChildren<Image>()[0].sprite = combatMoveTestComponent.getIconImage();
            item.GetComponentsInChildren<Image>()[1].sprite = combatMoveTestComponent.getIconImage();
            item.GetComponentsInChildren<TextMeshProUGUI>()[0].SetText(combatMoveTestComponent.GetName() + i);
            item.GetComponentsInChildren<TextMeshProUGUI>()[1].SetText(combatMoveTestComponent.GetPower().ToString());
            item.GetComponentsInChildren<TextMeshProUGUI>()[2].SetText(combatMoveTestComponent.GetCooldown().ToString());
            
            activeCombatMoves.Add(combatMoveTestComponent);
            item.transform.SetParent(rectTransform);
            item.transform.localScale = Vector2.one;
        }
        */
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

        Debug.Log("Current index: " + index);
        Debug.Log("Scrollcap: " + indexScrollCap);
        Debug.Log("Selector position: " + selectorPosition);
        
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
            if (index < maxIndex)
            {
                if (selectorPosition < indexScrollCap)
                {
                    transformPosition.y -= selectorMoveDistance;
                    selectorPosition++;
                }
                else if (selectorPosition == indexScrollCap)
                {
                    this.rectTransform.offsetMax -= new Vector2(0, -scrollOffset);
                }
                
                index++;
            }
            
        }
        
        selector.position = transformPosition;
        
    }

}
    
    
   
