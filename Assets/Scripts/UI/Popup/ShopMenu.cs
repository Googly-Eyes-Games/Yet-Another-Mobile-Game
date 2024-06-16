using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject itemTemplate;
    [SerializeField] private GameObject upgradeTemplate;
    
    [SerializeField] private GameObject boatsContent;
    [SerializeField] private GameObject linesContent;
    [SerializeField] private GameObject upgradesContent;
    
    [SerializeField] private List<ShopItemSO> boatItemsSO;
    [SerializeField] private List<ShopItemSO> lineItemsSO;
    [SerializeField] private List<ShopUpgradeSO> upgradesSO;
    
    private Dictionary<Button, ShopItemSO> buttonsDict = new();
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

    private void HandleItemClick(Button button, ShopItemSO shopItemSo)
    {
        GameSave newSave = SaveManager.Instance.Save;

        if (!shopItemSo.Purchased)
        {
            newSave.MoneyAmount -= shopItemSo.Price;
            SaveManager.Instance.SaveGameAsync(newSave);
            shopItemSo.Purchased = true;
        }
        
        if (shopItemSo.itemType == ShopItemSO.ItemType.Boat)
        {
            Player.GetComponentInChildren<SpriteRenderer>().sprite = shopItemSo.Sprite;
            newSave.SpriteName = shopItemSo.Sprite.name; 
        }
        else
        {
            var lineRenderer = Player.GetComponentInChildren<LineRenderer>();
            lineRenderer.startColor = shopItemSo.Color;
            lineRenderer.endColor = shopItemSo.Color;
            newSave.LineColor = shopItemSo.Color;
        }
            
        SaveManager.Instance.SaveGameAsync(newSave);
        button.GetComponentInChildren<TextMeshProUGUI>().text = "In use";
        button.interactable = false;
            
        foreach (var buttonPair in buttonsDict)
        {
            if (buttonPair.Key == button)
                continue;

            if (buttonPair.Value.itemType != shopItemSo.itemType)
                continue;

            if (!buttonPair.Value.Purchased) 
                continue;
            
            buttonPair.Key.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
            buttonPair.Key.interactable = true;
        }
    }
    
    
    private void CreateItem(ShopItemSO itemSO, Transform parentTransform, UnityEngine.Events.UnityAction<Button> onClickAction)
    {
        GameObject newItem = Instantiate(itemTemplate, parentTransform, false);
    
        ItemTemplate itemTemplateComponent = newItem.GetComponent<ItemTemplate>();
        
        itemTemplateComponent.titleText.text = itemSO.Title;
        itemTemplateComponent.image.sprite = itemSO.Sprite;
        itemTemplateComponent.image.color = itemSO.Color;
        itemTemplateComponent.priceText.text = $"Buy: {itemSO.Price} scrap";
    
        itemTemplateComponent.button.onClick.AddListener(() => onClickAction(itemTemplateComponent.button));
        
        buttonsDict.Add(itemTemplateComponent.button, itemSO);
        
        if (!itemSO.Purchased) 
            return;

        bool isInUse;

        if (itemSO.itemType == ShopItemSO.ItemType.Boat)
        {
            isInUse = itemSO.Sprite.name == SaveManager.Instance.Save.SpriteName;
        }
        else
        {
            isInUse = itemSO.Color == SaveManager.Instance.Save.LineColor;
        }

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
        
        if (upgradeSO.upgradeType == ShopUpgradeSO.UpgradeType.LineLength)
        {
            newSave.LineLengthLevel = upgradeSO.currentLevel;
            Player.GetComponent<RopeHandler>().UpgradeMarksCount(upgradeSO.UpgradeIncrement);
        }
        else
        {
            newSave.ShipSpeedLevel = upgradeSO.currentLevel;
            Player.GetComponent<PlayerMovement>().UpgradeShipSpeed(upgradeSO.UpgradeIncrement);
        }
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
            if (!buttonPair.Value.Purchased)
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
