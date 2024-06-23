using System;
using System.Collections;
using System.Collections.Generic;
using erulathra;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private SOEvent gameOverEvent;

    [SerializeField]
    private InputLockSOEvent inputLockSoEvent;
    
    [SerializeField]
    private TMP_Text scoreText; 

    [SerializeField]
    private GameObject highScoreText; 

    private void OnEnable()
    {
        gameOverEvent.OnRaise += HandleGameOver;
    }

    private void OnDisable()
    {
        gameOverEvent.OnRaise -= HandleGameOver;
    }

    void HandleGameOver()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.enabled = true;

        ScoreSubsystem scoreSubsystem = SceneSubsystemManager.GetSubsystem<ScoreSubsystem>();

        int score = scoreSubsystem.Score;

        GameSave newSave = SaveManager.Instance.Save;
        bool isNewRecord = score > newSave.HighScore;
        if (isNewRecord)
        {
            newSave.HighScore = score;
            SaveManager.Instance.SaveGameAsync(newSave);
        }

        scoreText.text = $"{score}";
        
        highScoreText.SetActive(isNewRecord);
        inputLockSoEvent?.Invoke(InputLock.Locked);
    }

    public void RestartLevel()
    {
        inputLockSoEvent?.Invoke(InputLock.None);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
