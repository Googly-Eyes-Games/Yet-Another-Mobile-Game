using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 0.5f;

    private void Update()
    {
        transform.position += Vector3.up * (Time.deltaTime * cameraSpeed);
    }
}
