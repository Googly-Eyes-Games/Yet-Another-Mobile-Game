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
    [SerializeField] private GameObject itemTemplate;
    [SerializeField] private GameObject upgradeTemplate;

    [SerializeField] private GameObject boatsContent;
    [SerializeField] private GameObject linesContent;
    [SerializeField] private GameObject upgradesContent;

    [SerializeField] private List<ShopUpgradeSO> upgradesSO;

    private Dictionary<Button, ShopItem> buttonsDict = new();
    private Dictionary<Button, ShopUpgradeSO> upgradesDict = new();

    private void Awake()
    {
        List<ShopItem> sortedItems = ShopItemsCollection.Instance.shopItemsDict.Values.ToList();
        sortedItems = sortedItems.OrderBy(item => item.Price).ToList();

        foreach (ShopItem shopItem in sortedItems)
        {
            if (shopItem.itemType == ShopItem.ItemType.Boat)
                CreateItem(shopItem, boatsContent.transform, button => HandleItemClick(button, shopItem));
            else
                CreateItem(shopItem, linesContent.transform, button => HandleItemClick(button, shopItem));

        }

        foreach (var upgradeSO in upgradesSO)
        {
            CreateUpgradeItem(upgradeSO, upgradesContent.transform,
                iconTemplate => HandleUpgradeClick(iconTemplate, upgradeSO));
        }
    }

    private void HandleItemClick(Button button, ShopItem shopItem)
    {
        GameSave newSave = SaveManager.Instance.Save;

        if (!newSave.WasItemBought(shopItem))
        {
            newSave.MoneyAmount -= shopItem.Price;
            newSave.BuyItem(shopItem);
        }

        button.GetComponentInChildren<TextMeshProUGUI>().text = "In use";
        button.interactable = false;

        foreach (var buttonPair in buttonsDict)
        {
            if (buttonPair.Key == button)
                continue;

            if (!newSave.WasItemBought(buttonPair.Value))
            {
                buttonPair.Key.interactable = buttonPair.Value.Price <= newSave.MoneyAmount;
                continue;
            }
            
            if (buttonPair.Value.itemType != shopItem.itemType)
                continue;
            
            buttonPair.Key.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
            buttonPair.Key.interactable = true;
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

        SetButtonState(itemTemplateComponent.button, isInUse);
    }

    private void SetButtonState(Button button, bool isInUse)
    {
        button.interactable = !isInUse;
        button.GetComponentInChildren<TextMeshProUGUI>().text = isInUse ? "In use" : "Use";
    }

    private void CreateUpgradeItem(ShopUpgradeSO upgradeSO, Transform parentTransform,
        UnityEngine.Events.UnityAction<ItemTemplate> onClickAction)
    {
        GameObject newItem = Instantiate(upgradeTemplate, parentTransform, false);

        ItemTemplate itemTemplateComponent = newItem.GetComponent<ItemTemplate>();

        itemTemplateComponent.titleText.text = upgradeSO.Title;
        itemTemplateComponent.image.sprite = upgradeSO.Sprite;

        GameSave newSave = SaveManager.Instance.Save;
        int currentUpgradeLevel = upgradeSO.upgradeType == ShopUpgradeSO.UpgradeType.RopeLength
            ? newSave.RopeLengthLevel
            : newSave.ShipSpeedLevel;

        itemTemplateComponent.priceText.text = currentUpgradeLevel == upgradeSO.maxLevel
            ? "Max"
            : $"Buy: ${upgradeSO.GetCurrentPrice(currentUpgradeLevel)}";

        itemTemplateComponent.upgradeText.text = currentUpgradeLevel.ToString();

        if (currentUpgradeLevel != upgradeSO.maxLevel)
        {
            itemTemplateComponent.button.interactable =
                upgradeSO.GetCurrentPrice(currentUpgradeLevel) <= newSave.MoneyAmount;
        }

        itemTemplateComponent.button.onClick.AddListener(() => onClickAction(itemTemplateComponent));
        upgradesDict.Add(itemTemplateComponent.button, upgradeSO);
    }

    private void HandleUpgradeClick(ItemTemplate iconTemplate, ShopUpgradeSO upgradeSO)
    {
        GameSave newSave = SaveManager.Instance.Save;

        int currentUpgradeLevel = upgradeSO.upgradeType == ShopUpgradeSO.UpgradeType.RopeLength
            ? newSave.RopeLengthLevel += 1
            : newSave.ShipSpeedLevel += 1;

        newSave.MoneyAmount -= upgradeSO.GetCurrentPrice(currentUpgradeLevel - 1);
        iconTemplate.upgradeText.text = currentUpgradeLevel.ToString();

        SaveManager.Instance.SaveGameAsync(newSave);

        foreach (var buttonPair in buttonsDict)
        {
            if (!newSave.BoughtItems.Contains(buttonPair.Value.ID))
            {
                buttonPair.Key.interactable = buttonsDict[buttonPair.Key].Price <= newSave.MoneyAmount;
            }
        }
        
        if (currentUpgradeLevel == upgradeSO.maxLevel)
        {
            iconTemplate.button.interactable = false;
            iconTemplate.button.GetComponentInChildren<TextMeshProUGUI>().text = "Max";
            return;
        }
        
        iconTemplate.button.GetComponentInChildren<TextMeshProUGUI>().text =
            $"Buy: ${upgradeSO.GetCurrentPrice(currentUpgradeLevel)}";
    }

    private void CheckUpgradesPrice()
    {
        GameSave newSave = SaveManager.Instance.Save;
        
        foreach (var upgradeKey in upgradesDict)
        {
            int currentUpgradeLevel = upgradeKey.Value.upgradeType == ShopUpgradeSO.UpgradeType.RopeLength
                ? newSave.RopeLengthLevel
                : newSave.ShipSpeedLevel;

            if (upgradeKey.Value.maxLevel != currentUpgradeLevel)
            {
                int upgradePrice = upgradeKey.Value.GetCurrentPrice(currentUpgradeLevel);
                upgradeKey.Key.interactable = upgradePrice <= newSave.MoneyAmount;
            }
        }
    }

    private void CheckItemsPrice()
    {
        GameSave newSave = SaveManager.Instance.Save;
        
        foreach (var buttonPair in buttonsDict)
        {
            if (!newSave.BoughtItems.Contains(buttonPair.Value.ID))
            {
                buttonPair.Key.interactable = buttonsDict[buttonPair.Key].Price <= newSave.MoneyAmount;
            }
        }
    }
    
    private void OnEnable()
    {
        CheckItemsPrice();
        CheckUpgradesPrice();
    }
}
