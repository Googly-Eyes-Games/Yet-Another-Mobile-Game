using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject shopContent;
    [SerializeField] private GameObject itemTemplate;
    [SerializeField] private List<ShopItemSO> ShopItemsSO;

    private void Start()
    {
        foreach (var shopItemSO in ShopItemsSO)
        {
            GameObject newItem = Instantiate(itemTemplate, shopContent.transform, false);

            ItemTemplate itemTemplateComponent = newItem.GetComponent<ItemTemplate>();
            
            itemTemplateComponent.titleText.text = shopItemSO.Title;
            itemTemplateComponent.image.sprite = shopItemSO.Sprite;
            itemTemplateComponent.image.color = shopItemSO.Color;
            itemTemplateComponent.priceText.text = shopItemSO.Price.ToString();
            
            itemTemplateComponent.button.onClick.AddListener( () => 
            {
                Player.GetComponentInChildren<SpriteRenderer>().color = shopItemSO.Color;
            });
        }
    }
    

    private void OnEnable() 
    {
        
        
    }

    private void OnDisable()
    {

    }
}
