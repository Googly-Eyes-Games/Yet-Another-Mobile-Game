using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : MonoBehaviour
{
    [SerializeField] private IntSOEvent onScoreChanged;

    [SerializeField] private Button muteButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button helpButton;
    [SerializeField] private Button etcButton;

    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject helpMenu;
    [SerializeField] private GameObject etcMenu;

    [SerializeField] private Sprite unmutedSprite;
    [SerializeField] private Sprite mutedSprite;

    private Image muteButtonImage;

    private void OnEnable()
    {
        muteButtonImage = muteButton.GetComponent<Image>();

        onScoreChanged.OnRaise += OnScoreChanged;
        muteButton.onClick.AddListener(OnMutePressed);
        shopButton.onClick.AddListener(OnShopPressed);
        helpButton.onClick.AddListener(OnHelpPressed);
        etcButton.onClick.AddListener(OnEtcPressed);
    }

    private void OnDisable()
    {
        onScoreChanged.OnRaise -= OnScoreChanged;
        muteButton.onClick.RemoveListener(OnMutePressed);
        shopButton.onClick.RemoveListener(OnShopPressed);
        helpButton.onClick.RemoveListener(OnHelpPressed);
        etcButton.onClick.RemoveListener(OnEtcPressed);
    }

    private void OnScoreChanged(int obj)
    {
        gameObject.SetActive(false);
    }

    private void OnMutePressed()
    {
        bool isMuted = AudioListener.volume == 0.0f;

        AudioListener.volume = isMuted ? 1.0f : 0.0f;
        muteButtonImage.sprite = isMuted ? unmutedSprite : mutedSprite;
    }

    private void OnShopPressed()
    {
        shopMenu.SetActive(!shopMenu.activeInHierarchy);
        helpMenu.SetActive(false);
        etcMenu.SetActive(false);
    }

    private void OnHelpPressed()
    {
        shopMenu.SetActive(false);
        helpMenu.SetActive(!helpMenu.activeInHierarchy);
        etcMenu.SetActive(false);
    }

    private void OnEtcPressed()
    {
        shopMenu.SetActive(false);
        helpMenu.SetActive(false);
        etcMenu.SetActive(!etcMenu.activeInHierarchy);
    }
}
