using System;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Add hajs");
            Destroy(gameObject);
        }
    }
}
