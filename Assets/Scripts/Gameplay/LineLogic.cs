using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    
    [SerializeField]
    private GameObject debugCircle;

    [SerializeField, Header("InputActions")]
    private InputActionProperty positionInputAction;
    
    [SerializeField]
    private InputActionProperty pressInputAction;

    private bool isPressed;

    private Vector2 position;

    private GameObject lastHit;
    private readonly List<GameObject> checkedNodes = new();

    private void Awake()
    {
        pressInputAction.action.started += PressStarted;
        pressInputAction.action.canceled += PressCancelled;
    }

    private void Update()
    {
        if (isPressed)
        {
            HandleDebugPoint();
            HandleLineLogic();
        }
    }

    private void HandleLineLogic()
    {
        Vector2 inputPoint = positionInputAction.action.ReadValue<Vector2>();
        
        Camera mainCam = Camera.main;
        if (mainCam)
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(inputPoint), Vector2.zero);
            if (hit)
            {
                GameObject hitGO = hit.collider.gameObject;

                if (hitGO != lastHit)
                {
                    lastHit = hitGO;
                
                    if (!checkedNodes.Contains(hitGO))
                    {
                        checkedNodes.Add(hitGO);
                        UpdateLineRenderer();
                    }
                    else if (checkedNodes[^2] == hitGO && checkedNodes.Count > 1)
                    {
                        checkedNodes.RemoveAt(checkedNodes.Count - 1);
                        UpdateLineRenderer();
                    }
                }
            }
        }
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = checkedNodes.Count;

        for (int nodeID = 0; nodeID < checkedNodes.Count; nodeID++)
        {
            lineRenderer.SetPosition(nodeID, checkedNodes[nodeID].transform.position);
        }
    }

    private void HandleDebugPoint()
    {
        RectTransform debugTransform = debugCircle.GetComponent<RectTransform>();
        position = positionInputAction.action.ReadValue<Vector2>();
        
        debugTransform.position = new Vector3(position.x, position.y, 999f);
    }

    public void PressStarted(InputAction.CallbackContext callbackContext)
    {
        isPressed = true;
    }
    
    public void PressCancelled(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("TapCanceled");
        isPressed = false;
        
        checkedNodes.Clear();
        UpdateLineRenderer();
    }
}
