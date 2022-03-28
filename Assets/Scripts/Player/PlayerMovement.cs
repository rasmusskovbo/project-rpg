using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isMoving;

    private Animator _animator;
    private Vector2 inputDirection;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _animator.SetBool("isMoving", isMoving);
        
        if (isMoving) return;
        if (inputDirection.Equals(Vector2.zero)) return;

        var targetPosition = transform.position;
        _animator.SetFloat("moveX", inputDirection.x);
        _animator.SetFloat("moveY", inputDirection.y);
            
        if (inputDirection.Equals(Vector2.left) || inputDirection.Equals(Vector2.right))
        {
            targetPosition.x += inputDirection.x;
        }
            
        if (inputDirection.Equals(Vector2.up) || inputDirection.Equals(Vector2.down))
        {
            targetPosition.y += inputDirection.y;
        }

        StartCoroutine(MovePlayer(targetPosition));
        
    }

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
}
