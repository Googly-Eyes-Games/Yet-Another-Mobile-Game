using UnityEngine;

public class OilValidator : MonoBehaviour
{
    [SerializeField]
    private SOEvent onGameOver;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActiveAndEnabled && other.CompareTag("Oil"))
        {
            onGameOver?.Invoke();
        }
    }
}
