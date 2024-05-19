using System;
using UnityEngine;
using UnityTimer;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    private float timeToDestroy = 5f;

    private void Start()
    {
        Timer.Register(timeToDestroy, () =>
        {
            Destroy(gameObject);
        }, autoDestroyOwner: this);
    }
}
