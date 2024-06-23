using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField]
    private IntSOEvent onScoreChanged;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private ScoreHandler scoreHandler;

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
        if (canvas.enabled == false)
        {
            canvas.enabled = true;
            scoreHandler.OnShowScore();
        }
    }
}
