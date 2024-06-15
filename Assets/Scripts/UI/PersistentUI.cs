using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    [Foldout("Bindings")]
    [SerializeField]
    private TMP_Text moneyText;

    [Foldout("Events")]
    [SerializeField]
    private SOEvent saveDataChangedEvent;

    [Foldout("Components")]
    [SerializeField]
    private ShopMenu shopMenuComponent;
    
    private void OnEnable()
    {
        saveDataChangedEvent.OnRaise += HandleSaveDataChanged;
        HandleSaveDataChanged();
    }

    private void OnDisable()
    {
        saveDataChangedEvent.OnRaise -= HandleSaveDataChanged;
    }

    private void HandleSaveDataChanged()
    {
        moneyText.text = $"{SaveManager.Instance.Save.MoneyAmount}";
    }
    
    private void OnApplicationQuit()
    {
        shopMenuComponent.GetComponent<ShopMenu>().ResetSO();
    }
}
