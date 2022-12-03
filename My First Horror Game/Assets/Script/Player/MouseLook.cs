using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("References")]
    private PlayerInput _playerInputController;
    private Camera _playerCamera;

    [Header("rotation")]
    [SerializeField, Range(1, 10)] private float _lookSpeedHorizontal = 2.0f;
    [SerializeField, Range(1, 10)] private float _lookSpeedVertical = 2.0f;
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
        _playerCamera = this.GetComponent<Camera>();
    }
    private void Update()
    {

        HandleMouseInput();
    }


    //The reason for using local Rotation here is:
    // Use local rotation, camera component will rotate depends on the parent component-player body
    // When use say turn to the left, you are using your own body as a pivot point
    private void HandleMouseInput()
    {
        _mouseInput = _playerInputController.Player.Cameralooking.ReadValue<Vector2>();

        _rotationVerticalRotateDegree -= _mouseInput.y * _lookSpeedVertical;
        _rotationVerticalRotateDegree = Mathf.Clamp(_rotationVerticalRotateDegree, -_lowerLookLimit, _upperLookLimit);

        //rotate camera up and down
        _playerCamera.transform.localRotation = Quaternion.Euler(_rotationVerticalRotateDegree, 0, 0);

        //rotate player body and camera left and right
        //this will rotate the parent body, not the camera
        transform.rotation *= Quaternion.Euler(0, _mouseInput.x * _lookSpeedHorizontal, 0);
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
