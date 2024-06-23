using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextPulseAnimation : MonoBehaviour
{
    [SerializeField]
    private Color highlightColor;

    [SerializeField]
    private Vector2 sizeRange;
    
    [SerializeField]
    private Color dimColor;

    [SerializeField]
    private float animationSpeed = 1f;

    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        float animationTime = Mathf.Sin(Time.time * animationSpeed) * 0.5f + 0.5f;
        text.color = Color.Lerp(dimColor, highlightColor, animationTime);
        transform.localScale = Vector3.one * Mathf.Lerp(sizeRange.x, sizeRange.y, animationTime);
    }
}
