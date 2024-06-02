using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private IntSOEvent onScoreChanged;
    [SerializeField] private Canvas canvas;

    private void OnEnable()
    {
        onScoreChanged.OnRaise += OnScoreChanged;
    }

    private void OnDisable()
    {
        onScoreChanged.OnRaise -= OnScoreChanged;
    }

    private void OnScoreChanged(int obj)
    {
        canvas.enabled = true;
    }

    public void OnPauseButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
