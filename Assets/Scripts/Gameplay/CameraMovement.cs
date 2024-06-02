using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private SOEvent startGameEvent;
    
    [SerializeField]
    private SOEvent gameOverEvent;
    
    [SerializeField]
    private float cameraTargetStartSpeed = 2f;
    
    [SerializeField]
    private float cameraStartAcceleration = 1f;
    
    [SerializeField]
    private float cameraAcceleration = 0.1f;

    private bool GameStarted = false;

    private float currentCameraSpeed = 0f;
    private float currentAcceleration = 0f;

    private void Update()
    {
        if (GameStarted)
        {
            transform.position += Vector3.up * (Time.deltaTime * currentCameraSpeed);
            
            float speedNormalized = Mathf.InverseLerp(0, cameraTargetStartSpeed, currentCameraSpeed);
            currentAcceleration = Mathf.Lerp(cameraStartAcceleration, cameraAcceleration, speedNormalized);
            
            currentCameraSpeed += currentAcceleration * Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        startGameEvent.OnRaise += StartGame;
        gameOverEvent.OnRaise += GameOver;
    }

    private void OnDisable()
    {
        startGameEvent.OnRaise -= StartGame;
        gameOverEvent.OnRaise -= GameOver;
    }

    public void StartGame()
    {
        GameStarted = true;
    }
    
    private void GameOver()
    {
        GameStarted = false;
    }

}
