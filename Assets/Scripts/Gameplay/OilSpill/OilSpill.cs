using System;
using UnityEngine;

public class OilSpill : MonoBehaviour
{
    public event Action OnCleaned;

    public void FullClean()
    {
        OnCleaned?.Invoke();
    }
}