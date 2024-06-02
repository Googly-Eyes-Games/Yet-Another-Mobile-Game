using UnityEngine;

public class Scrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameSave newSave = SaveManager.Instance.Save;
            newSave.MoneyAmount += 1;
            SaveManager.Instance.SaveGameAsync(newSave);
            
            Destroy(gameObject);
        }
    }
}
