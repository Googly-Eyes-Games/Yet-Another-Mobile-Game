using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class HighScoreText : MonoBehaviour
{
    private TMP_Text text;
    
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        GameSave save = SaveManager.Instance.Save;
        text.text = $"{save.HighScore}";
    }
}
