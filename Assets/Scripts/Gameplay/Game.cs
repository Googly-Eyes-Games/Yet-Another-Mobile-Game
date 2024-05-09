using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject debugCircle;

    [SerializeField, Header("InputActions")]
    private InputActionProperty positionInputAction;
    
    [SerializeField]
    private InputActionProperty pressInputAction;

    private bool isPressed;

    private Vector2 startPos;
    private Vector2 position;

    private void Awake()
    {
        pressInputAction.action.started += PressStarted;
        pressInputAction.action.canceled += PressCancelled;
    }

    private void Update()
    {
        if (isPressed)
        {
            RectTransform debugTransform = debugCircle.GetComponent<RectTransform>();
            position = positionInputAction.action.ReadValue<Vector2>();
            
            debugTransform.position = new Vector3(position.x, position.y, 999f);
            
            Debug.Log($"Pos: {position}, DeltaPos: {position - startPos}");
        }
    }

    public void SetStartPosition()
    {
        startPos = position;
        Debug.Log($"StartPos: {startPos}");
    }

    public void PressStarted(InputAction.CallbackContext callbackContext)
    {
        SetStartPosition();
        isPressed = true;
    }
    
    public void PressCancelled(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("TapCanceled");
        isPressed = false;
    }
}
