using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SFGPlayerInput : MonoBehaviour
{
    public event Action OnPress;
    public event Action OnUnPress;
    
    public Vector2 TouchPosition => positionInputAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
    public bool IsPressed { get; private set; }
    
    [SerializeField]
    private InputActionProperty positionInputAction;
    
    [SerializeField]
    private InputActionProperty pressInputAction;

    [SerializeField]
    private InputLockSOEvent inputLockSoEvent;

    [field: SerializeField]
    public Collider bottomScreenCollider { get; private set; }

    public InputLock CurrentInputLock { get; private set; } = InputLock.LockOnlyBottom;

    private void OnEnable()
    {
        pressInputAction.action.started += PressStarted;
        pressInputAction.action.canceled += PressCancelled;

        inputLockSoEvent.OnRaise += HandleInputLock;
    }

    private void OnDisable()
    {
        pressInputAction.action.started -= PressStarted;
        pressInputAction.action.canceled -= PressCancelled;
        
        inputLockSoEvent.OnRaise -= HandleInputLock;
    }

    private void PressStarted(InputAction.CallbackContext callbackContext)
    {
        IsPressed = true;
        OnPress?.Invoke();
    }

    private void PressCancelled(InputAction.CallbackContext callbackContext)
    {
        IsPressed = false;
        OnUnPress?.Invoke();
    }

    private void HandleInputLock(InputLock inputLock)
    {
        CurrentInputLock = inputLock;
    }

}