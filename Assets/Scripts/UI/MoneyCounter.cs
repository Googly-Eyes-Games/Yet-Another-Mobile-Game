using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField]
    private float animationTime = 0.5f;
    
    [SerializeField]
    private TMP_Text moneyText;

    [SerializeField]
    private SOEvent saveDataChangedEvent;

    [SerializeField]
    private Color highLightColor;
    
    private Color defaultColor;
    private float defautlFontSize;
    
    private void OnEnable()
    {
        defaultColor = moneyText.color;
        defautlFontSize = moneyText.fontSize;
        
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
        DOTween.To(
            () => defautlFontSize * 1.3f,
            x => moneyText.fontSize = x,
            defautlFontSize, animationTime);
        
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(moneyText.DOColor(highLightColor, animationTime * 0.5f))
            .Append(moneyText.DOColor(defaultColor, animationTime * 0.5f))
            .Play();
        
    }
}
