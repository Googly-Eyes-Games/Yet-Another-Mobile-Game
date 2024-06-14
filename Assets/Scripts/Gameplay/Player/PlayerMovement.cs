using System;
using UnityEngine;

[RequireComponent(typeof(SFGPlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float shipSpeed = 10f;
    
    [SerializeField]
    private float distanceToCalculateRotation = 0.1f;
    
    private SFGPlayerInput playerInput;

    private Vector3 targetPosition;
    private Vector3 lastPosition;

    private void Awake()
    {
        playerInput = GetComponent<SFGPlayerInput>();
        
        Camera mainCamera = Camera.main;
        targetPosition = mainCamera.WorldToScreenPoint(transform.position);
        
        lastPosition = transform.position + Vector3.down;
    }

    private void Update()
    {
        UpdateTargetPosition();
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdateTargetPosition()
    {
        if (playerInput.IsPressed)
        {
            targetPosition = playerInput.TouchPosition;
        }
    }

    private void UpdatePosition()
    {
        Camera mainCamera = Camera.main;
        Vector3 worldTargetPosition = mainCamera.ScreenToWorldPoint(targetPosition);
        worldTargetPosition.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, worldTargetPosition, Time.deltaTime * shipSpeed);
    }

    private void UpdateRotation()
    {
        Vector3 deltaPosition = transform.position - lastPosition;
        if (deltaPosition.magnitude > distanceToCalculateRotation)
        {
            lastPosition = transform.position;
            Vector3 boatDirection = deltaPosition.normalized;
            transform.rotation = MathUtils.LookAt2D(boatDirection);
        }
    }
    
    public void UpgradeShipSpeed(int value)
    {
        shipSpeed += value;
    }
}
