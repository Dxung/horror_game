using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMovement : MonoBehaviour
{
    

    [Header("Movement Parameters")]
    private bool _canMove = true;
    [SerializeField] private float _moveSpeed = 0f;
    [SerializeField] private float _gravity = 9.8f;
    private Vector3 _moveDirection;


    [Header("References")]
    private CharacterController _playerCharacterController;
    private PlayerInput _playerInputController;

    
    [Header("Input Processes")]
    private Vector2 _rawCurrentInput;
    private Vector2 _smoothInputToApplyThisTime;
    private Vector2 _smoothInputVelocity;
    [SerializeField] private float _smoothInputSpeed=.2f;


    private void Awake()
    {
        _playerCharacterController = this.GetComponentInParent<CharacterController>();
        _playerInputController = new PlayerInput();
    }

    private void Start()
    {
        _playerCharacterController.skinWidth = 0.04f;
    }

    private void Update()
    {
        //calculate move direction  --> check gravity & change direction.y --> apply move with calculated direction
        //HandleMovementInput()     --> HandleGravity()                    --> ApplyFinalMovement()
        if (_canMove)
        {
            HandleMovementInput();
            ApplyFinalMovement();
        }
    }

    private void HandleMovementInput()
    {

        // _smoothInputToApplyThisTime : the vector2 "position" that character move the last time
        // _rawCurrentInput            : the vector2 "position" that character should move to this time

        _rawCurrentInput = _playerInputController.Player.Movement.ReadValue<Vector2>();
        _smoothInputToApplyThisTime = Vector2.SmoothDamp(_smoothInputToApplyThisTime, _rawCurrentInput, ref _smoothInputVelocity, _smoothInputSpeed);

        float moveDirectionY = _moveDirection.y;

        //change player's moving direction from local to world 
        _moveDirection = (transform.TransformDirection(Vector3.forward) * _smoothInputToApplyThisTime.y) + (transform.TransformDirection(Vector3.right) * _smoothInputToApplyThisTime.x);
        _moveDirection.y = moveDirectionY;

    }

    private void ApplyFinalMovement()
    {

        HandleGravity();

        _playerCharacterController.Move(_moveDirection * _moveSpeed * Time.deltaTime);
    }



    //Handle Gravity
    private void HandleGravity()
    {
        if (!_playerCharacterController.isGrounded)
        {
            _moveDirection.y -= _gravity * Time.deltaTime;
        }
    }

    //Apply Jump
    public void JumpButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("jump");
    }

    //enable/disable Input System
    private void OnEnable()
    {
        _playerInputController.Enable();
        _playerInputController.Player.Jump.started += JumpButtonPressed;
    }


    private void OnDisable()
    {
        _playerInputController.Disable();
    }

    //getter & setter 
    public bool CanPlayerMove()
    {
        return _canMove;
    }

    private void AllowOrNotPlayerMove(bool canPlayerMove)
    {
        _canMove = canPlayerMove;
    }

    
    
}
