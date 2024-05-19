using System;
using UnityEngine;

public class SOEvent<TArgs> : ScriptableObject
{
    public event Action<TArgs> OnRaise;

    public void Invoke(TArgs arg)
    {
        OnRaise?.Invoke(arg);
    }
}

[CreateAssetMenu(fileName = "SOE_BasicEvent", menuName = "ScriptableEvents/Basic", order = 0)]
public class SOEvent : ScriptableObject
{
    public event Action OnRaise;

    public void Invoke()
    {
        OnRaise?.Invoke();
    }
}