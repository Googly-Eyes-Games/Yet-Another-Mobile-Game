using System;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField]
    private IntSOEvent onScoreChanged;

    [SerializeField]
    private TMP_Text text;

    public void OnEnable()
    {
        onScoreChanged.OnRaise += HandleScoreChanged;
    }

    private void OnDisable()
    {
        onScoreChanged.OnRaise -= HandleScoreChanged;
    }

    public void HandleScoreChanged(int score)
    {
        text.text = $"Score: {score}";
    }
}