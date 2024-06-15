using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MainMenuScreen : MonoBehaviour
{
    [SerializeField] private IntSOEvent onScoreChanged;
    [SerializeField] private InputLockSOEvent onInputLockChanged;

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

        onInputLockChanged.Invoke(InputLock.LockOnlyBottom);

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
        onInputLockChanged.Invoke(InputLock.None);
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
        bool show = !shopMenu.activeInHierarchy;

        shopMenu.SetActive(show);
        helpMenu.SetActive(false);
        etcMenu.SetActive(false);
        
        onInputLockChanged.Invoke(show ? InputLock.Locked : InputLock.LockOnlyBottom);
    }

    private void OnHelpPressed()
    {
        bool show = !helpMenu.activeInHierarchy;

        shopMenu.SetActive(false);
        helpMenu.SetActive(show);
        etcMenu.SetActive(false);

        onInputLockChanged.Invoke(show ? InputLock.Locked : InputLock.LockOnlyBottom);
    }

    private void OnEtcPressed()
    {
        bool show = !etcMenu.activeInHierarchy;

        shopMenu.SetActive(false);
        helpMenu.SetActive(false);
        etcMenu.SetActive(show);

        onInputLockChanged.Invoke(show ? InputLock.Locked : InputLock.LockOnlyBottom);
    }
}
