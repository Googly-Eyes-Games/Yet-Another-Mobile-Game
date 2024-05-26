using System;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField]
    private IntSOEvent onScoreChanged;

    [SerializeField]
    private TMP_Text text;

    // Wrong place to keep score. Just for testing
    private int score;

    public void OnEnable()
    {
        onScoreChanged.OnRaise += HandleScoreChanged;
    }

    private void OnDisable()
    {
        onScoreChanged.OnRaise -= HandleScoreChanged;
    }

    public void HandleScoreChanged(int scoreDifference)
    {
        score += scoreDifference;
        text.text = $"Score: {score}";
    }
}