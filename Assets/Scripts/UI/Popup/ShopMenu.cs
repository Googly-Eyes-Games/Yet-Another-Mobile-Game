using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject shopContent;
    [SerializeField] private GameObject itemTemplate;
    [SerializeField] private List<ShopItemSO> ShopItemsSO;


    private void Awake()
    {
        foreach (var shopItemSO in ShopItemsSO)
        {
            GameObject newItem = Instantiate(itemTemplate);
            
            itemTemplate.GetComponent<ItemTemplate>().titleText.text = shopItemSO.Title;
            itemTemplate.GetComponent<ItemTemplate>().image.sprite = shopItemSO.Sprite;
            itemTemplate.GetComponent<ItemTemplate>().priceText.text = shopItemSO.Price.ToString();
            
            newItem.transform.SetParent(shopContent.transform, false);

        }
    }

    private void OnEnable() 
    {
        
        
    }

    private void OnDisable()
    {

    }
}
