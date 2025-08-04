using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier;
    private PlayerActions inputActions;
    private void OnEnable()
    {
        inputActions?.Enable();
    }

    private void OnDisable()
    {
        inputActions?.Disable();
    }
    private void Awake()
    {
        inputActions = new PlayerActions();
    }
    private void FixedUpdate()
    {
        Vector2 input = inputActions.Movement.Move.ReadValue<Vector2>();
        Vector3 move = new Vector3(-input.x, 0f, input.y);
        float sprintSpeed = inputActions.Movement.Run.IsPressed() ? sprintMultiplier : 1;
        _rigidbody.MovePosition(_rigidbody.position + move * (moveSpeed * sprintSpeed) * Time.fixedDeltaTime);
    }

}
