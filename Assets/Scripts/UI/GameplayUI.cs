using TMPro;
using UnityEngine;
using UnityTimer;

public class GameplayUI : MonoBehaviour
{
    [SerializeField]
    private BoardLogic boardLogic;

    [SerializeField]
    private TMP_Text gameplayText;

    private void Awake()
    {
        boardLogic.OnGameFinish += HandleGameFinish;
    }

    private void HandleGameFinish(bool isSolutionValid)
    {
        gameplayText.enabled = true;
        
        if (isSolutionValid)
        {
            gameplayText.text = "Valid Solution";
            gameplayText.color = Color.green;
        }
        else
        {
            gameplayText.text = "Invalid Solution";
            gameplayText.color = Color.red;
        }

        Timer.Register(2f, () => { gameplayText.enabled = false; });
    }
}
