using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject itemTemplate;
    
    [SerializeField] private GameObject boatsContent;
    [SerializeField] private GameObject linesContent;
    
    [SerializeField] private List<ShopItemSO> boatItemsSO;
    [SerializeField] private List<ShopItemSO> lineItemsSO;

    private Dictionary<Button, ShopItemSO> buttonsDict = new();
    private Dictionary<ShopItemSO, bool> itemsDict = new();

    private void Awake()
    {
        GameSave newSave = SaveManager.Instance.Save;
        
        foreach (var boatItemSO in boatItemsSO)
        {
            CreateItem(boatItemSO, boatsContent.transform, () =>
            {
                Button button = buttonsDict.FirstOrDefault(x => x.Value == boatItemSO).Key;
                if (button == null)
                    return;

                if (itemsDict[boatItemSO])
                {
                    button.interactable = false;
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "In use";
                    Player.GetComponentInChildren<SpriteRenderer>().sprite = boatItemSO.Sprite;
                    return;
                }

                newSave.MoneyAmount -= boatItemSO.Price;
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
                itemsDict[boatItemSO] = true;

            });
        }

        foreach (var lineItemSO in lineItemsSO)
        {
            CreateItem(lineItemSO, linesContent.transform, () =>
            {
                var lineRenderer = Player.GetComponentInChildren<LineRenderer>();
                lineRenderer.startColor = lineItemSO.Color;
                lineRenderer.endColor = lineItemSO.Color;
            });
        }
    }
    
    private void CreateItem(ShopItemSO itemSO, Transform parentTransform, UnityEngine.Events.UnityAction onClickAction)
    {
        GameObject newItem = Instantiate(itemTemplate, parentTransform, false);
    
        ItemTemplate itemTemplateComponent = newItem.GetComponent<ItemTemplate>();
        
        itemTemplateComponent.titleText.text = itemSO.Title;
        itemTemplateComponent.image.sprite = itemSO.Sprite;
        itemTemplateComponent.image.color = itemSO.Color;
        itemTemplateComponent.priceText.text = itemSO.Price.ToString();
    
        itemTemplateComponent.button.onClick.AddListener(onClickAction);
        buttonsDict.Add(itemTemplateComponent.button, itemSO);
        itemsDict.Add(itemSO, itemSO.Purchased);

        if (!itemSO.Purchased) 
            return;
        
        itemTemplateComponent.button.interactable = false;
        itemTemplateComponent.button.GetComponentInChildren<TextMeshProUGUI>().text = "In use";
        
    }
    
    
    private void OnEnable() 
    {
        GameSave newSave = SaveManager.Instance.Save;
        
        foreach (var button in buttonsDict.Keys)
        {
            button.interactable = buttonsDict[button].Price <= newSave.MoneyAmount;
        }
        
    }

    private void OnDisable()
    {

    }
}
