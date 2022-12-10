using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("References")]
    private PlayerInput _playerInputController;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private GameObject _playerBody;

    [Header("rotation")]
    [SerializeField, Range(0, 500)] private float _lookSpeedHorizontal = 200f;
    [SerializeField, Range(0, 500)] private float _lookSpeedVertical = 200f;
    [SerializeField, Range(1, 180)] private float _upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float _lowerLookLimit = 80.0f;
    private float _rotationVerticalRotateDegree = 0;

    [Header("Mouse Input")]
    private Vector2 _mouseInput;

    private void Awake()
    {
        _playerInputController = new PlayerInput();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {

        HandleMouseInput();
        _mouseInput = _playerInputController.Player.Cameralooking.ReadValue<Vector2>();
    }

    /// <summary>
    /// mouse moving on screen is 2D values
    ///values apply to character is in 3D 

    ///x value (HORIZONTAL) in 2D ~~ Y Axis (for rotate HORIZONTAL) in 3D
    ///y value (VERTICAL) in 2D   ~~ X Axis (for rotate VERTICAL) in 3D
    /// </summary>


    //The reason for using local Rotation here is:
    // Use local rotation, camera component will rotate depends on the parent component-player body
    // When use say turn up, you are using your own body as a pivot point
    private void HandleMouseInput()
    {
        _mouseInput = _playerInputController.Player.Cameralooking.ReadValue<Vector2>();

        _rotationVerticalRotateDegree -= _mouseInput.y * _lookSpeedVertical*Time.deltaTime;
        _rotationVerticalRotateDegree = Mathf.Clamp(_rotationVerticalRotateDegree, -_lowerLookLimit, _upperLookLimit);

        //rotate camera up and down
        _playerCamera.transform.localRotation = Quaternion.Euler(_rotationVerticalRotateDegree, 0, 0);

        //rotate player body and camera left and right
        //this will rotate the parent body, not the camera
        _playerBody.transform.Rotate(Vector3.up * _mouseInput.x * _lookSpeedHorizontal*Time.deltaTime);
    }

    private void OnEnable()
    {
        _playerInputController.Enable();
    }

    private void OnDisable()
    {
        _playerInputController.Disable();
    }

    
}
