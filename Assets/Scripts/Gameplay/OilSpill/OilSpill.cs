using System;
using erulathra;
using UnityEngine;

public class OilSpill : MonoBehaviour
{
    public event Action OnCleaned;

    public void FullClean()
    {
        OnCleaned?.Invoke();
    }

    public void OnEnable()
    {
        OilSpillsSubsystem oilSpillsSubsystem = SceneSubsystemManager.GetSubsystem<OilSpillsSubsystem>();
        oilSpillsSubsystem.RegisterSpill(this);
    }

    public void OnDisable()
    {
        OilSpillsSubsystem oilSpillsSubsystem = SceneSubsystemManager.GetSubsystem<OilSpillsSubsystem>();
        oilSpillsSubsystem.UnregisterSpill(this);
    }
}