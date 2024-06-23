using System;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class SFXManager : MonoBehaviour
{
    [Foldout("Events")]
    [SerializeField]
    private SOEvent gameOverEvent;
    
    [Foldout("Events")]
    [SerializeField]
    private SOEvent cleanEvent;
    
    [Foldout("Events")]
    [SerializeField]
    private SOEvent wrongCleanEvent;
    
    [Foldout("Events")]
    [SerializeField]
    private SOEvent scrapCollectedEvent;
    
    [Foldout("Audio Clips")]
    [SerializeField]
    private AudioClip cleanAC;

    [SerializeField]
    private Vector2 splashPitchRange;
    
    [Foldout("Audio Clips")]
    [SerializeField]
    private AudioClip wrongCleanAC;
    
    [Foldout("Audio Clips")]
    [SerializeField]
    private AudioClip coinAC;
    
    [Foldout("Audio Clips")]
    [SerializeField]
    private AudioClip gameOverAC;

    private AudioSource cleanAS;
    private AudioSource wrongCleanAS;
    private AudioSource coinAS;
    private AudioSource gameOverAS;

    private void Awake()
    {
        cleanAS = InitializeAudioSource(cleanAC);
        wrongCleanAS = InitializeAudioSource(wrongCleanAC);
        coinAS = InitializeAudioSource(coinAC);
        gameOverAS = InitializeAudioSource(gameOverAC);
    }

    private AudioSource InitializeAudioSource(AudioClip audioClip)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.playOnAwake = false;
        return audioSource;
    }

    private void OnEnable()
    {
        gameOverEvent.OnRaise += HandleGameOver;
        cleanEvent.OnRaise += HandleClean;
        wrongCleanEvent.OnRaise += HandleWrongClean;
        scrapCollectedEvent.OnRaise += HandleScrapCollected;
    }

    private void OnDisable()
    {
        gameOverEvent.OnRaise -= HandleGameOver;
        cleanEvent.OnRaise -= HandleClean;
        wrongCleanEvent.OnRaise -= HandleWrongClean;
        scrapCollectedEvent.OnRaise -= HandleScrapCollected;
    }

    private void HandleGameOver()
    {
        gameOverAS.Play();
    }

    private void HandleClean()
    {
        cleanAS.pitch = Random.Range(splashPitchRange.x, splashPitchRange.y);
        cleanAS.Play();
    }
    
    private void HandleWrongClean()
    {
        wrongCleanAS.Play();
        HandleClean();
    }

    private void HandleScrapCollected()
    {
        coinAS.Play();
    }

}
