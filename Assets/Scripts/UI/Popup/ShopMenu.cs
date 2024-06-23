using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] 
    private GameObject itemTemplate;
    
    [SerializeField] 
    private GameObject upgradeTemplate;

    [SerializeField] 
    private GameObject boatsContent;
    
    [SerializeField] 
    private GameObject linesContent;
    
    [SerializeField] 
    private GameObject upgradesContent;

    [SerializeField] 
    private List<ShopUpgradeSO> upgradesSO;

    [SerializeField] 
    private SOEvent onReset;
    
    [SerializeField] 
    private ColorButtonManager colorManager;
    
    private Dictionary<Button, ShopItem> buttonsDict = new();
    private Dictionary<Button, ShopUpgradeSO> upgradesDict = new();

    private void Awake()
    {
        InitializeItems();
        onReset.OnRaise += HandleReset;
    }
    
    private void OnEnable()
    {
        CheckItemsPrice();
        CheckUpgradesPrice();
    }

    private void HandleItemClick(Button button, ShopItem shopItem)
    {
        GameSave newSave = SaveManager.Instance.Save;

        if (!newSave.WasItemBought(shopItem))
        {
            newSave.MoneyAmount -= shopItem.Price;
            newSave.BuyItem(shopItem);
        }
        
        SetButtonState(button, false);
        
        foreach (var buttonPair in buttonsDict)
        {
            if (buttonPair.Key == button)
                continue;

            if (!newSave.WasItemBought(buttonPair.Value))
            {
                buttonPair.Key.interactable = buttonPair.Value.Price <= newSave.MoneyAmount;
                colorManager.ChangeButtonAppearance(buttonPair.Key);
                continue;
            }
            
            if (buttonPair.Value.itemType != shopItem.itemType)
                continue;
            
            SetButtonState(buttonPair.Key, true, "Use");
        }

        CheckUpgradesPrice();

        if (shopItem.itemType == ShopItem.ItemType.Boat)
            newSave.BoatItemInUse = shopItem.ID;
        else if (shopItem.itemType == ShopItem.ItemType.Line)
            newSave.LineItem = shopItem.ID;

        SaveManager.Instance.SaveGameAsync(newSave);
    }
    
    private void CreateItem(ShopItem item, Transform parentTransform,
        UnityEngine.Events.UnityAction<Button> onClickAction)
    {
        GameObject newItem = Instantiate(itemTemplate, parentTransform, false);

        ItemTemplate itemTemplateComponent = newItem.GetComponent<ItemTemplate>();

        itemTemplateComponent.titleText.text = item.Title;
        itemTemplateComponent.image.sprite = item.Sprite;
        itemTemplateComponent.image.color = item.Color;

        itemTemplateComponent.priceText.text = $"Buy: ${item.Price}";

        itemTemplateComponent.button.onClick.AddListener(() => onClickAction(itemTemplateComponent.button));

        buttonsDict.Add(itemTemplateComponent.button, item);

        GameSave save = SaveManager.Instance.Save;

        int currentBoughtItems = SaveManager.Instance.Save.BoughtItems.Count;

        if (currentBoughtItems < 3 && item.Price == 0)
        {
            save.BuyItem(item);

            if (item.itemType == ShopItem.ItemType.Boat)
                save.BoatItemInUse = item.ID;
            else if (item.itemType == ShopItem.ItemType.Line)
                save.LineItem = item.ID;
            
            SaveManager.Instance.SaveGameAsync(save);
        }

        if (!save.WasItemBought(item))
        {
            itemTemplateComponent.button.interactable = item.Price <= save.MoneyAmount;
            return;
        }

        bool isInUse = item.ID == SaveManager.Instance.Save.BoatItemInUse
                       || item.ID == SaveManager.Instance.Save.LineItem;

        string currentUseText = isInUse ? "In use" : "Use"; 
        
        SetButtonState(itemTemplateComponent.button, !isInUse, currentUseText);
    }

    private void SetButtonState(Button button, bool isInteractable, string text = "In use")
    {
        button.interactable = isInteractable;
        colorManager.ChangeButtonAppearance(button);
        button.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    private void CreateUpgradeItem(ShopUpgradeSO upgradeSO, Transform parentTransform,
        UnityEngine.Events.UnityAction<ItemTemplate> onClickAction)
    {
        GameObject newItem = Instantiate(upgradeTemplate, parentTransform, false);

        ItemTemplate itemTemplateComponent = newItem.GetComponent<ItemTemplate>();

        itemTemplateComponent.titleText.text = upgradeSO.Title;
        
        GameSave newSave = SaveManager.Instance.Save;
        int currentUpgradeLevel = upgradeSO.upgradeType == ShopUpgradeSO.UpgradeType.RopeLength
            ? newSave.RopeLengthLevel
            : newSave.ShipSpeedLevel;

        itemTemplateComponent.priceText.text = currentUpgradeLevel == upgradeSO.maxLevel
            ? "Max"
            : $"Buy: ${upgradeSO.GetCurrentPrice(currentUpgradeLevel)}";

        itemTemplateComponent.upgradeText.text = currentUpgradeLevel.ToString();

        if (itemTemplateComponent.slider)
        {
            foreach (var sliderImage in itemTemplateComponent.slider.GetComponentsInChildren<Image>())
            {
                sliderImage.sprite = upgradeSO.Sprite;
            }

            itemTemplateComponent.slider.maxValue = upgradeSO.maxLevel;
            itemTemplateComponent.slider.value = currentUpgradeLevel;
        }
        else
        {
            itemTemplateComponent.image.sprite = upgradeSO.Sprite;
        }
        
        if (currentUpgradeLevel != upgradeSO.maxLevel)
        {
            itemTemplateComponent.button.interactable =
                upgradeSO.GetCurrentPrice(currentUpgradeLevel) <= newSave.MoneyAmount;
        }
        else
        {
            itemTemplateComponent.button.interactable = false;
        }
        
        colorManager.ChangeButtonAppearance(itemTemplateComponent.button);
        itemTemplateComponent.button.onClick.AddListener(() => onClickAction(itemTemplateComponent));
        upgradesDict.Add(itemTemplateComponent.button, upgradeSO);
    }

    private void HandleUpgradeClick(ItemTemplate upgraderTemplate, ShopUpgradeSO upgradeSO)
    {
        GameSave newSave = SaveManager.Instance.Save;

        int currentUpgradeLevel = upgradeSO.upgradeType == ShopUpgradeSO.UpgradeType.RopeLength
            ? newSave.RopeLengthLevel += 1
            : newSave.ShipSpeedLevel += 1;

        int currentPrice = upgradeSO.GetCurrentPrice(currentUpgradeLevel);
        
        newSave.MoneyAmount -= currentPrice - 1;
        upgraderTemplate.upgradeText.text = currentUpgradeLevel.ToString();
        upgraderTemplate.slider.value = currentUpgradeLevel;

        SaveManager.Instance.SaveGameAsync(newSave);

        if (currentUpgradeLevel == upgradeSO.maxLevel)
        {
            SetButtonState(upgraderTemplate.button, false, "Max");
        }
        else
        {
            bool isInteractable = currentPrice <= newSave.MoneyAmount;
            SetButtonState(upgraderTemplate.button, isInteractable, $"Buy: ${currentPrice}");
        }
        
        foreach (var buttonPair in buttonsDict)
        {
            if (newSave.BoughtItems.Contains(buttonPair.Value.ID)) 
                continue;
            
            buttonPair.Key.interactable = buttonsDict[buttonPair.Key].Price <= newSave.MoneyAmount;
            colorManager.ChangeButtonAppearance(buttonPair.Key);
        }

        foreach (var upgradePair in upgradesDict)
        {
            if (upgradePair.Value == upgradeSO)
                continue;
            
            int upgradeLevel = upgradePair.Value.upgradeType == ShopUpgradeSO.UpgradeType.RopeLength
                ? newSave.RopeLengthLevel
                : newSave.ShipSpeedLevel;

            if (upgradePair.Value.maxLevel == upgradeLevel) 
                continue;
            
            int upgradePrice = upgradePair.Value.GetCurrentPrice(upgradeLevel);
            upgradePair.Key.interactable = upgradePrice <= newSave.MoneyAmount;
            colorManager.ChangeButtonAppearance(upgradePair.Key);
            
        }
    }

    private void CheckUpgradesPrice()
    {
        GameSave newSave = SaveManager.Instance.Save;
        
        foreach (var upgradeKey in upgradesDict)
        {
            int currentUpgradeLevel = upgradeKey.Value.upgradeType == ShopUpgradeSO.UpgradeType.RopeLength
                ? newSave.RopeLengthLevel
                : newSave.ShipSpeedLevel;

            if (upgradeKey.Value.maxLevel == currentUpgradeLevel) 
                continue;
            
            int upgradePrice = upgradeKey.Value.GetCurrentPrice(currentUpgradeLevel);
            
            bool isInteractable = upgradePrice <= newSave.MoneyAmount;
            
            SetButtonState(upgradeKey.Key, isInteractable, $"Buy: ${upgradePrice}");
        }
    }

    private void CheckItemsPrice()
    {
        GameSave save = SaveManager.Instance.Save;
        
        foreach (var buttonPair in buttonsDict)
        {
            if (!save.BoughtItems.Contains(buttonPair.Value.ID))
            {
                buttonPair.Key.interactable = buttonsDict[buttonPair.Key].Price <= save.MoneyAmount;
                colorManager.ChangeButtonAppearance(buttonPair.Key);
            }
        }
    }

    private void InitializeItems()
    {
        List<ShopItem> sortedItems = ShopItemsCollection.Instance.shopItemsDict.Values.ToList();
        sortedItems = sortedItems.OrderBy(item => item.Price).ToList();
        
        foreach (ShopItem shopItem in sortedItems)
        {
            if (!boatsContent || !linesContent) 
                continue;
            
            if (shopItem.itemType == ShopItem.ItemType.Boat)
                CreateItem(shopItem, boatsContent.transform, button => HandleItemClick(button, shopItem));
            else
                CreateItem(shopItem, linesContent.transform, button => HandleItemClick(button, shopItem));

        }

        if (!upgradesContent) 
            return;
        
        foreach (var upgradeSO in upgradesSO)
        {
            CreateUpgradeItem(upgradeSO, upgradesContent.transform,
                iconTemplate => HandleUpgradeClick(iconTemplate, upgradeSO));
        }
    }

    private void HandleReset()
    {
        DestroyItem(boatsContent);
        DestroyItem(linesContent);
        DestroyItem(upgradesContent);
        
        buttonsDict.Clear();
        upgradesDict.Clear();

        InitializeItems();
    }

    private void DestroyItem(GameObject parent)
    {
        if (!parent)
            return;
        
        foreach (var item in parent.GetComponentsInChildren<ItemTemplate>())
        {
            Destroy(item.gameObject);
        }
    }
}
