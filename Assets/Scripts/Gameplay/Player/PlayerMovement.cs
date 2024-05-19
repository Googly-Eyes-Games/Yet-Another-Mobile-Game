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
        targetPosition = transform.position;
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
            Camera camera = Camera.main;
            Vector3 worldTouchPosition = camera.ScreenToWorldPoint(playerInput.TouchPosition);
            worldTouchPosition.z = 0f;

            targetPosition = worldTouchPosition;
        }
    }

    private void UpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * shipSpeed);
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
}
