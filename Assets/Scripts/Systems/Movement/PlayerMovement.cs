using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isMoving;
    [SerializeField] private LayerMask blockingLayer;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private LayerMask combatLayer;
    
    private Animator animator;
    private UIExplController uiController;
    private CombatEncounterManager _combatEncounterManager;
    
    private Vector2 inputDirection;
    private PlayerFacing playerFacingDirection;
    
    // Cached animator references
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        uiController = FindObjectOfType<UIExplController>();
        _combatEncounterManager = FindObjectOfType<CombatEncounterManager>();
    }

    void Update()
    {
        animator.SetBool(IsMoving, isMoving);
        
        if (isMoving) return;
        if (inputDirection.Equals(Vector2.zero)) return;

        var targetPosition = transform.position;
        animator.SetFloat(MoveX, inputDirection.x);
        animator.SetFloat(MoveY, inputDirection.y);
            
        if (inputDirection.Equals(Vector2.left) || inputDirection.Equals(Vector2.right))
        {
            targetPosition.x += inputDirection.x;
            UpdatePlayerFacing();
        }
            
        if (inputDirection.Equals(Vector2.up) || inputDirection.Equals(Vector2.down))
        {
            targetPosition.y += inputDirection.y;
            UpdatePlayerFacing();
        }

        if (IsTargetPositionWalkable(targetPosition)) StartCoroutine(MovePlayer(targetPosition));
        
    }

    public void UpdatePlayerFacing()
    {
        if (inputDirection.x != 0)
        {
            playerFacingDirection = inputDirection.x > 0 ? PlayerFacing.East : PlayerFacing.West;    
        }
        else if (inputDirection.y != 0)
        {
            playerFacingDirection = inputDirection.y > 0 ? PlayerFacing.North : PlayerFacing.South;    
        }
        
        Debug.Log(playerFacingDirection);
    }

    public void SetPlayerFacing(PlayerFacing direction)
    {
        switch (direction)
        {
            case PlayerFacing.North:
                animator.SetFloat(MoveX, 0);
                animator.SetFloat(MoveY, 1);
                break;
            case PlayerFacing.South:
                animator.SetFloat(MoveX, 0);
                animator.SetFloat(MoveY, -1);
                break;
            case PlayerFacing.East:
                animator.SetFloat(MoveX, 1);
                animator.SetFloat(MoveY, 0);
                break;
            case PlayerFacing.West:
                animator.SetFloat(MoveX, -1);
                animator.SetFloat(MoveY, 0);
                break;
        }
    }

    // Input system reference.
    void OnMove(InputValue input)
    {
        inputDirection = input.Get<Vector2>();
    }

    void OnOpenCharacterStats()
    {
        uiController.ToggleCharacterStats();
    }
    
    void OnOpenInventory()
    {
        uiController.ToggleInventory();
    }

    IEnumerator MovePlayer(Vector3 targetPosition)
    {
        isMoving = true;

        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
        
        FindObjectOfType<GameManager>().SavePositionBeforeCombat();
        CheckForCombat();
    }

    private bool IsTargetPositionWalkable(Vector3 targetPosition)
    {
        return !Physics2D.OverlapCircle(targetPosition, 0.15f, blockingLayer) 
               && !Physics2D.OverlapCircle(targetPosition, 0.15f, waterLayer);
    }

    private void CheckForCombat()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, combatLayer) != null)
        {
            if ((Random.Range(1, 101) <= _combatEncounterManager.AreaEncounterRate))
            {
                Debug.Log("Encountered combat!");
                FindObjectOfType<GameManager>().SavePositionBeforeCombat();
                SceneManager.LoadScene(1);
            }
        }
    }

    public PlayerFacing PlayerFacingDirection
    {
        get => playerFacingDirection;
        set => playerFacingDirection = value;
    }
}
