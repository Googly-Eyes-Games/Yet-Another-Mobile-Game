using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        foreach (var boatItemSO in boatItemsSO)
        {
            CreateItem(boatItemSO, boatsContent.transform, () => 
            {
                Player.GetComponentInChildren<SpriteRenderer>().color = boatItemSO.Color;
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
    }
    
    
    
    private void OnEnable() 
    {
        
        
    }

    private void OnDisable()
    {

    }
}
