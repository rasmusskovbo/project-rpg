using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isMoving;
    [SerializeField] private LayerMask blockingLayer;
    [SerializeField] private LayerMask waterLayer;

    private Animator animator;
    private Vector2 inputDirection;
    
    // Cached animator references
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        }
            
        if (inputDirection.Equals(Vector2.up) || inputDirection.Equals(Vector2.down))
        {
            targetPosition.y += inputDirection.y;
        }

        if (IsTargetPositionWalkable(targetPosition)) StartCoroutine(MovePlayer(targetPosition));
        
    }

    // Input system reference.
    void OnMove(InputValue input)
    {
        inputDirection = input.Get<Vector2>();
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
    }

    private bool IsTargetPositionWalkable(Vector3 targetPosition)
    {
        return !Physics2D.OverlapCircle(targetPosition, 0.15f, blockingLayer) 
               && !Physics2D.OverlapCircle(targetPosition, 0.15f, waterLayer);
    }
}
