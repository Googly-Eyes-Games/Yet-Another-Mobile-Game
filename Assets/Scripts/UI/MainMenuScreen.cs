using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour
{
    [SerializeField] private IntSOEvent onScoreChanged;

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
        gameObject.SetActive(false);
    }
}
