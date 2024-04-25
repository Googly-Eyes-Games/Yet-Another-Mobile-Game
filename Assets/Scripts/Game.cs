using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField]
    private TMP_Text gameText;
    
    [SerializeField]
    private Button mainButton;

    private void Awake()
    {
        if (mainButton != null)
        {
            mainButton.onClick.AddListener(OnGameButtonClicked);
        }
        
        gameText.gameObject.SetActive(false);
    }

    private void OnGameButtonClicked()
    {
        gameText.gameObject.SetActive(true);
    }
}
