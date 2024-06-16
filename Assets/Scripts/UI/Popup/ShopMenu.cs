using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject itemTemplate;
    [SerializeField] private GameObject upgradeTemplate;
    
    [SerializeField] private GameObject boatsContent;
    [SerializeField] private GameObject linesContent;
    [SerializeField] private GameObject upgradesContent;
    
    [SerializeField] private List<ShopItem> boatItemsSO;
    [SerializeField] private List<ShopItem> lineItemsSO;
    [SerializeField] private List<ShopUpgradeSO> upgradesSO;
    
    private Dictionary<Button, ShopItem> buttonsDict = new();
    private Dictionary<Button, ShopUpgradeSO> upgradesDict = new();
    
    private void Awake()
    {
        foreach (var boatItemSO in boatItemsSO)
        {
            CreateItem(boatItemSO, boatsContent.transform,  button  => HandleItemClick(button, boatItemSO));
        }

        foreach (var lineItemSO in lineItemsSO)
        {
            CreateItem(lineItemSO, linesContent.transform, button => HandleItemClick(button, lineItemSO));
        }
        
        foreach (var upgradeSO in upgradesSO)
        {
            CreateUpgradeItem(upgradeSO, upgradesContent.transform, iconTemplate => HandleUpgradeClick(iconTemplate, upgradeSO));
        }
    }

    private void HandleItemClick(Button button, ShopItem shopItem)
    {
        GameSave newSave = SaveManager.Instance.Save;

        if (!newSave.BoughtItems.Contains(shopItem.ID))
        {
            newSave.MoneyAmount -= shopItem.Price;
            shopItem.Purchased = true;

            newSave.BoughtItems.Add(shopItem.ID);
        }
        
        button.GetComponentInChildren<TextMeshProUGUI>().text = "In use";
        button.interactable = false;
            
        foreach (var buttonPair in buttonsDict)
        {
            if (buttonPair.Key == button)
                continue;

            if (buttonPair.Value.itemType != shopItem.itemType)
                continue;

            if (!buttonPair.Value.Purchased) 
                continue;
            
            buttonPair.Key.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
            buttonPair.Key.interactable = true;
        }

        if (shopItem.itemType == ShopItem.ItemType.Boat)
            newSave.BoatItemInUse = shopItem.ID;
        else if (shopItem.itemType == ShopItem.ItemType.Line)
            newSave.LineItem = shopItem.ID;
        
        SaveManager.Instance.SaveGameAsync(newSave);
    }
    
    
    private void CreateItem(ShopItem item, Transform parentTransform, UnityEngine.Events.UnityAction<Button> onClickAction)
    {
        GameObject newItem = Instantiate(itemTemplate, parentTransform, false);
    
        ItemTemplate itemTemplateComponent = newItem.GetComponent<ItemTemplate>();
        
        itemTemplateComponent.titleText.text = item.Title;
        itemTemplateComponent.image.sprite = item.Sprite;
        itemTemplateComponent.image.color = item.Color;
        itemTemplateComponent.priceText.text = $"Buy: {item.Price} scrap";
    
        itemTemplateComponent.button.onClick.AddListener(() => onClickAction(itemTemplateComponent.button));
        
        buttonsDict.Add(itemTemplateComponent.button, item);
        
        if (!item.Purchased) 
            return;

        bool isInUse = item.ID == SaveManager.Instance.Save.BoatItemInUse
                       || item.ID == SaveManager.Instance.Save.LineItem;

        SetButtonState(itemTemplateComponent.button, isInUse);
    }
    
    private void SetButtonState(Button button, bool isInUse)
    {
        button.interactable = !isInUse;
        button.GetComponentInChildren<TextMeshProUGUI>().text = isInUse ? "In use" : "Use";
    }
    
    private void CreateUpgradeItem(ShopUpgradeSO upgradeSO, Transform parentTransform, UnityEngine.Events.UnityAction<ItemTemplate> onClickAction)
    {
        GameObject newItem = Instantiate(upgradeTemplate, parentTransform, false);

        ItemTemplate itemTemplateComponent = newItem.GetComponent<ItemTemplate>();

        itemTemplateComponent.titleText.text = upgradeSO.Title;
        itemTemplateComponent.image.sprite = upgradeSO.Sprite;
        itemTemplateComponent.priceText.text = upgradeSO.currentLevel == upgradeSO.maxLevel ? "Max" : $"Buy: {upgradeSO.GetCurrentPrice()} scrap";
        itemTemplateComponent.upgradeText.text = upgradeSO.currentLevel.ToString();

        itemTemplateComponent.button.onClick.AddListener(() => onClickAction(itemTemplateComponent));
        upgradesDict.Add(itemTemplateComponent.button, upgradeSO);
    }
    
    private void HandleUpgradeClick(ItemTemplate iconTemplate, ShopUpgradeSO upgradeSO)
    {
        GameSave newSave = SaveManager.Instance.Save;

        newSave.MoneyAmount -= upgradeSO.GetCurrentPrice();
        upgradeSO.currentLevel += upgradeSO.UpgradeIncrement;
        
        SaveManager.Instance.SaveGameAsync(newSave);
        
        iconTemplate.upgradeText.text = upgradeSO.currentLevel.ToString();
        
        if (upgradeSO.currentLevel == upgradeSO.maxLevel)
        {
            iconTemplate.button.interactable = false;
            iconTemplate.button.GetComponentInChildren<TextMeshProUGUI>().text = "Max";
            return;
        }

        iconTemplate.button.GetComponentInChildren<TextMeshProUGUI>().text = $"Buy: {upgradeSO.GetCurrentPrice()} scrap";
    }
    
    private void OnEnable() 
    {
        GameSave newSave = SaveManager.Instance.Save;
        
        foreach (var buttonPair in buttonsDict)
        {
            if (!newSave.BoughtItems.Contains(buttonPair.Value.ID))
            {
                buttonPair.Key.interactable = buttonsDict[buttonPair.Key].Price <= newSave.MoneyAmount;
            }
        }
        
        foreach (var upgradePair in upgradesDict)
        {
            if (upgradePair.Value.currentLevel == upgradePair.Value.maxLevel ||
                upgradePair.Value.GetCurrentPrice() > newSave.MoneyAmount)
            {
                upgradePair.Key.interactable = false;
            }
            else
            {
                upgradePair.Key.interactable = true;
            }
        }
    }

    public void ResetSO()
    {
        foreach (var itemSo in buttonsDict.Values)
        {
            itemSo.Purchased = itemSo.Price == 0;
        }
        
        foreach (var upgradeSo in upgradesDict.Values)
        {
            upgradeSo.currentLevel = 0;
        }
    }
    
    private void OnDisable()
    {

    }
}
