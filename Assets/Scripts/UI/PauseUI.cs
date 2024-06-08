using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Button resumeButton;

    [SerializeField]
    private Button restartButton;

    private void OnEnable() 
    {
        resumeButton.onClick.AddListener(OnResumeButton);
        restartButton.onClick.AddListener(OnRestartButton);
    }
    
    private void OnDisable()
    {
        resumeButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
    }

    public void OnPauseButton()
    {
        bool pause = Time.timeScale > 0.0f;
        Time.timeScale = pause ? 0.0f : 1.0f;
        canvas.enabled = pause ? true : false;
    }

    public void OnResumeButton()
    {
        Time.timeScale = 1.0f;
        canvas.enabled = false;
    }

    public void OnRestartButton()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
