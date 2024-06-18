using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorButtonManager : MonoBehaviour
{
    public Color interactableButtonColor;
    public Color nonInteractableButtonColor;

    public Color interactableTextColor;
    public Color nonInteractableTextColor;

    public void ChangeButtonAppearance(Button button)
    {
        Image buttonImage = button.gameObject.GetComponent<Image>();
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        
        if (!buttonImage && !buttonText)
            return;

        bool interactable = button.interactable;
        
        buttonImage.color = interactable ? interactableButtonColor : nonInteractableButtonColor;
        buttonText.color = interactable ? interactableTextColor : nonInteractableTextColor;
    }
    
    public void DisableButtonAppearance(Button button)
    {
        if (button == null)
            return;

        button.interactable = false; 
        ChangeButtonAppearance(button);
    }
    
    public void EnableButtonAppearance(Button button)
    {
        if (button == null)
            return;
        
        button.interactable = true; 
        ChangeButtonAppearance(button);
    }
}
    

