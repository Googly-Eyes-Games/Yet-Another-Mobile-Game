using UnityEngine;

public class Scrap : MonoBehaviour
{
    [SerializeField]
    private SOEvent scrapCollectedEvent;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameSave newSave = SaveManager.Instance.Save;
            newSave.MoneyAmount += 1;
            SaveManager.Instance.SaveGameAsync(newSave);

            scrapCollectedEvent?.Invoke();
            
            Destroy(gameObject);
        }
    }
}
