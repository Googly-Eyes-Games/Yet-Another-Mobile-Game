using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityTimer;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField]
    private IntSOEvent onScoreChanged;

    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private float animationTime = 0.9f;

    [SerializeField]
    private float showScoreDelay = 2.0f;

    private float fontSize;
    private float positionY;

    private void Start() 
    {
        fontSize = text.fontSize;
        positionY = text.rectTransform.localPosition.y;
        text.rectTransform.localPosition = new Vector2(0, 60);
    }

    public void OnEnable()
    {
        onScoreChanged.OnRaise += HandleScoreChanged;
    }

    private void OnDisable()
    {
        onScoreChanged.OnRaise -= HandleScoreChanged;
    }

    public void OnShowScore()
    {
        DOTween.To(() => text.rectTransform.localPosition.y, y => text.rectTransform.localPosition = new Vector2(0, y), positionY + 620, 2.0f).SetEase(Ease.OutElastic);
    }

    public void HandleScoreChanged(int score)
    {
        DOTween.To(() => fontSize * 1.3f, x => text.fontSize = x, fontSize, animationTime);
        DOTween.To(() => int.Parse(text.text), x => text.text = $"{x}", score, animationTime);
    }
}