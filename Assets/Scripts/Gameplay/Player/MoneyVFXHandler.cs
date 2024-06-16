using System;
using UnityEngine;

public class MoneyVFXHandler : MonoBehaviour
{
    [SerializeField]
    private SOEvent scrapCollectedEvent;

    private ParticleSystem moneyParticleSystem;

    public void Awake()
    {
        moneyParticleSystem = GetComponent<ParticleSystem>();
    }

    public void OnEnable()
    {
        scrapCollectedEvent.OnRaise += HandleScrapCollected;
    }

    public void OnDisable()
    {
        scrapCollectedEvent.OnRaise -= HandleScrapCollected;
    }

    private void HandleScrapCollected()
    {
        moneyParticleSystem.Play();
    }
}
