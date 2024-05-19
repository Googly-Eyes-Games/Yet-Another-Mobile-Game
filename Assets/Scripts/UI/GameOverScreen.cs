using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private SOEvent gameOverEvent;

    private void OnEnable()
    {
        gameOverEvent.OnRaise += HandleGameOver;
    }

    private void OnDisable()
    {
        gameOverEvent.OnRaise -= HandleGameOver;
    }

    void HandleGameOver()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.enabled = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
