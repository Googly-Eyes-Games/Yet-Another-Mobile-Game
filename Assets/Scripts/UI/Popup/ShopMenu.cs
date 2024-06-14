using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    
    private Dictionary<Button, ShopItemSO> boatsDict = new();
    private Dictionary<Button, ShopItemSO> linesDict = new();
    private Dictionary<Button, ShopItemSO> buttonsDict = new();
    private Dictionary<ShopItemSO, bool> itemsDict = new();

    private List<ShopUpgradeSO> startShopUpgradesSO;
    
    private Dictionary<Button, ShopUpgradeSO> upgradesDict = new();
    
    private void Awake()
    {
        GameSave newSave = SaveManager.Instance.Save;
        newSave.MoneyAmount = 50;
        SaveManager.Instance.SaveGameAsync(newSave);
        
        foreach (var boatItemSO in boatItemsSO)
        {
            CreateItem(boatItemSO, boatsContent.transform,  button  => HandleItemClick(button, boatItemSO, true));
        }

        foreach (var lineItemSO in lineItemsSO)
        {
            CreateItem(lineItemSO, linesContent.transform, button => HandleItemClick(button, lineItemSO, false));
        }
        
        foreach (var upgradeSO in upgradesSO)
        {
            CreateUpgradeItem(upgradeSO, upgradesContent.transform, iconTemplate => HandleUpgradeClick(iconTemplate, upgradeSO));
        }
    }

    private void HandleItemClick(Button button, ShopItemSO shopItemSo,  bool isBoat)
    {
        GameSave newSave = SaveManager.Instance.Save;
        
        if (itemsDict[shopItemSo])
        {
            if (isBoat)
            {
                Player.GetComponentInChildren<SpriteRenderer>().sprite = shopItemSo.Sprite;
            }
            else
            {
                var lineRenderer = Player.GetComponentInChildren<LineRenderer>();
                lineRenderer.startColor = shopItemSo.Color;
                lineRenderer.endColor = shopItemSo.Color;
            }
            
            button.GetComponentInChildren<TextMeshProUGUI>().text = "In use";
            button.interactable = false;

            Dictionary<Button, ShopItemSO> localDict = isBoat ? boatsDict : linesDict;
            
            foreach (var buttonPair in localDict)
            {
                if (buttonPair.Key == button)
                    continue;

                if (!itemsDict[buttonPair.Value]) 
                    continue;
                        
                buttonPair.Key.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
                buttonPair.Key.interactable = true;
            }
        }
        else
        {
            newSave.MoneyAmount -= shopItemSo.Price;
            SaveManager.Instance.SaveGameAsync(newSave);
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
            itemsDict[shopItemSo] = true;
        }
    }
    
    
    private void CreateItem(ShopItemSO itemSO, Transform parentTransform, UnityEngine.Events.UnityAction<Button> onClickAction)
    {
        GameObject newItem = Instantiate(itemTemplate, parentTransform, false);
    
        ItemTemplate itemTemplateComponent = newItem.GetComponent<ItemTemplate>();
        
        itemTemplateComponent.titleText.text = itemSO.Title;
        itemTemplateComponent.image.sprite = itemSO.Sprite;
        itemTemplateComponent.image.color = itemSO.Color;
        itemTemplateComponent.priceText.text = $"{itemSO.Price} $";
    
        itemTemplateComponent.button.onClick.AddListener(() => onClickAction(itemTemplateComponent.button));

        if (parentTransform == boatsContent.transform)
        {
            boatsDict.Add(itemTemplateComponent.button, itemSO);
        }
        else
        {
            linesDict.Add(itemTemplateComponent.button, itemSO);
        }
        
        buttonsDict.Add(itemTemplateComponent.button, itemSO);
        itemsDict.Add(itemSO, itemSO.Purchased);

        if (!itemSO.Purchased) 
            return;
        
        itemTemplateComponent.button.interactable = false;
        itemTemplateComponent.button.GetComponentInChildren<TextMeshProUGUI>().text = "In use";
    }
    
    private void CreateUpgradeItem(ShopUpgradeSO upgradeSO, Transform parentTransform, UnityEngine.Events.UnityAction<ItemTemplate> onClickAction)
    {
        GameObject newItem = Instantiate(upgradeTemplate, parentTransform, false);

        ItemTemplate itemTemplateComponent = newItem.GetComponent<ItemTemplate>();

        itemTemplateComponent.titleText.text = upgradeSO.Title;
        itemTemplateComponent.image.sprite = upgradeSO.Sprite;
        itemTemplateComponent.priceText.text = $"{upgradeSO.BasePrice} $";
        itemTemplateComponent.upgradeText.text = upgradeSO.currentLevel.ToString();

        itemTemplateComponent.button.onClick.AddListener(() => onClickAction(itemTemplateComponent));
        upgradesDict.Add(itemTemplateComponent.button, upgradeSO);
    }
    
    private void HandleUpgradeClick(ItemTemplate iconTemplate, ShopUpgradeSO upgradeSO)
    {
        GameSave newSave = SaveManager.Instance.Save;

        newSave.MoneyAmount -= upgradeSO.GetCurrentPrice();
        SaveManager.Instance.SaveGameAsync(newSave);
        
        if (upgradeSO.upgradeType == ShopUpgradeSO.UpgradeType.LineLength)
        {
            Player.GetComponent<RopeHandler>().UpgradeMarksCount(upgradeSO.UpgradeIncrement);
        }
        else
        {
            Player.GetComponent<PlayerMovement>().UpgradeShipSpeed(upgradeSO.UpgradeIncrement);
        }
        
        upgradeSO.currentLevel += upgradeSO.UpgradeIncrement;
        iconTemplate.upgradeText.text = upgradeSO.currentLevel.ToString();
        
        if (upgradeSO.currentLevel == upgradeSO.maxLevel)
        {
            iconTemplate.button.interactable = false;
            iconTemplate.button.GetComponentInChildren<TextMeshProUGUI>().text = "Max";
            return;
        }
        
        iconTemplate.button.GetComponentInChildren<TextMeshProUGUI>().text = $"{upgradeSO.GetCurrentPrice()} $";
        
    }
    
    private void OnEnable() 
    {
        GameSave newSave = SaveManager.Instance.Save;
        
        foreach (var buttonPair in buttonsDict)
        {
            if (!itemsDict[buttonPair.Value])
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
        foreach (var upgradeSo in upgradesDict.Values)
        {
            upgradeSo.currentLevel = 0;
        }
    }
    
    private void OnDisable()
    {

    }
}
