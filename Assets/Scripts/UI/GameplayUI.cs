using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField]
    private IntSOEvent onScoreChanged;
    
    [SerializeField]
    private SOEvent onGameOver;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private ScoreHandler scoreHandler;

    private void OnEnable()
    {
        onScoreChanged.OnRaise += OnScoreChanged;
        onGameOver.OnRaise += OnGameOver;
    }

    private void OnDisable()
    {
        onScoreChanged.OnRaise -= OnScoreChanged;
        onGameOver.OnRaise -= OnGameOver;
    }
    
    private void OnGameOver()
    {
        canvas.enabled = false;
    }

    private void OnScoreChanged(int obj)
    {
        if (canvas.enabled == false)
        {
            canvas.enabled = true;
            scoreHandler.OnShowScore();
        }
    }
}
