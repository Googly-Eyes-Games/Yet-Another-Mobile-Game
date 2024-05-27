using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuScreen : MonoBehaviour
{
    [SerializeField] private IntSOEvent onScoreChanged;
    [SerializeField] private Button muteButton;

    private void OnEnable()
    {
        onScoreChanged.OnRaise += OnScoreChanged;
        muteButton.onClick.AddListener(OnMutePressed);
    }

    private void OnDisable()
    {
        onScoreChanged.OnRaise -= OnScoreChanged;
        muteButton.onClick.RemoveListener(OnMutePressed);
    }

    private void OnScoreChanged(int obj)
    {
        gameObject.SetActive(false);
    }

    private void OnMutePressed()
    {
        bool isMuted = AudioListener.volume == 0.0f;
        AudioListener.volume = isMuted ? 1.0f : 0.0f;
    }

    private void OnShopPressed()
    {
        //TODO
    }

    private void OnHelpPressed()
    {
        //TODO
    }
}
