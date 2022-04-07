using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class UITargetSelect : MonoBehaviour
{
    private CombatSystem combatSystem;
    
    [Header("Cursor")] 
    [SerializeField] private Transform cursor;
    [SerializeField] private Sprite cursorIdle;
    [SerializeField] private Sprite cursorOnSelect;
    
    [Header("Settings")]
    [SerializeField] private float xCursorOffset;
    [SerializeField] private float yCursorOffset;
    
    // Selecting
    private Vector2 defaultSelectorPosition;
    private Vector2 inputDirection;
    private int index;
    private int maxIndex;
    
    // Valid positions
    private List<Vector2> selectablePositions;

    private void Awake()
    {
        selectablePositions = new List<Vector2>();
    }

    private void Start()
    {
        combatSystem = FindObjectOfType<CombatSystem>();
        foreach (Transform transform in combatSystem.GetActiveEnemyStations())
        {
            Vector2 modifiedTransform = transform.position;
            modifiedTransform.x += xCursorOffset;
            modifiedTransform.y += yCursorOffset;
            selectablePositions.Add(modifiedTransform);
        }

        combatSystem.GetActiveEnemyStations();
        
        defaultSelectorPosition = selectablePositions[0];
        cursor.position = defaultSelectorPosition;
        index = 0;
        maxIndex = selectablePositions.Count - 1;
    }

    void OnMoveSelector(InputValue value)
    {
        Debug.Log("INPUT");
        Debug.Log("Current index: " + index);
        Debug.Log("Max Index: " + maxIndex);
        
        if (combatSystem.State != CombatState.PLAYER_TARGET_SELECT) return;
        if (maxIndex < 1) return;
        
        inputDirection = value.Get<Vector2>();
        
        if (inputDirection.Equals(Vector2.up))
        {
            if (index > 0)
            {
                cursor.position = selectablePositions[index - 1];
                index--;
            }
        }

        if (inputDirection.Equals(Vector2.down))
        {
            if (index < maxIndex)
            {
                cursor.position = selectablePositions[index + 1];
                index++;
            }
        }

        if (inputDirection.Equals(Vector2.right))
        {
            // If less than 4 monsters, disable controls.
            // If on the rightmost places in a group of 4, disable controls
            // If on the rightmost places in a group of 5, disable controls
            if (maxIndex < 3) return;
            if (maxIndex == 3 && index >= 2) return;
            if (maxIndex == 4 && index >= 3) return;
            
            if ((maxIndex == 4 && index == 2) || maxIndex == 3)
            {
                cursor.position = selectablePositions[index + 2];
                index += 2;
            }
            else if (maxIndex == 4)
            {
                cursor.position = selectablePositions[index + 3];
                index += 3;    
            }

        }
        
        if (inputDirection.Equals(Vector2.left))
        {
            // If less than 4 monsters, disable controls.
            // If on the leftmost places in a group of 4, disable controls
            // If on the leftmost places in a group of 5, disable controls
            if (maxIndex < 3) return;
            if (maxIndex == 3 && index <= 1) return;
            if (maxIndex == 4 && index <= 2) return;
            
            if (maxIndex == 3)
            {
                cursor.position = selectablePositions[index - 2];
                index -= 2;
            }
            else if (maxIndex == 4)
            {
                cursor.position = selectablePositions[index - 3];
                index -= 3;    
            }
        }



    }
}
