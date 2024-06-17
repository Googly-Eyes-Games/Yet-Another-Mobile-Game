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

    public void ChangeButtonAppearance(Button button, bool interactable)
    {
        Image buttonImage = button.gameObject.GetComponent<Image>();
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        
        if (!buttonImage && !buttonText)
            return;
        
        buttonImage.color = interactable ? interactableButtonColor : nonInteractableButtonColor;
        buttonText.color = interactable ? interactableTextColor : nonInteractableTextColor;
    }
    
    public void DisableButtonApperance(GameObject gameObject)
    {
        Button button = gameObject.GetComponent<Button>();

        if (button == null)
            return;
        
        ChangeButtonAppearance(button, false);
    }
    
    public void EnableButtonApperance(GameObject gameObject)
    {
        Button button = gameObject.GetComponent<Button>();

        if (button == null)
            return;
        
        ChangeButtonAppearance(button, true);
    }
}
    

