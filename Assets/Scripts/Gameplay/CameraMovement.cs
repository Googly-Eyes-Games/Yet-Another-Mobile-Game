using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float cameraStartSpeed = 2f;
    
    [SerializeField]
    private float cameraAcceleration = 0.1f;

    private void Update()
    {
        transform.position += Vector3.up * (Time.deltaTime * cameraStartSpeed);
        cameraStartSpeed += cameraAcceleration * Time.deltaTime;
    }
}
