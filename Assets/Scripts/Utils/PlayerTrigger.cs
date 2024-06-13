using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField]
    private bool oneShot = true;

    [SerializeField]
    private UnityEvent OnTrigger;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            OnTrigger?.Invoke();
            if (oneShot) gameObject.SetActive(false);
        }
    }
}
