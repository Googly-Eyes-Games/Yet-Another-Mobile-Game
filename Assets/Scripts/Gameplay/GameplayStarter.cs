using System;
using UnityEngine;

[RequireComponent(typeof(OilSpill))]
public class GameplayStarter : MonoBehaviour
{
    [SerializeField]
    private SOEvent startGameplayEvent;

    private OilSpill oilSpill;

    private void Awake()
    {
        oilSpill = GetComponent<OilSpill>();
    }

    private void OnEnable()
    {
        oilSpill.OnCleaned += StartGameplay;
    }

    private void OnDisable()
    {
        oilSpill.OnCleaned -= StartGameplay;
    }

    private void StartGameplay()
    {
        startGameplayEvent?.Invoke();
    }
}