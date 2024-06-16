using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    public class ShopItemsCollection : ScriptableObject
    {
        [SerializeField]
        ShopItem[] shopItems;

        public Dictionary<string, ShopItem> shopItemsDict { get; private set; }

        private static ShopItemsCollection instance;
        public static ShopItemsCollection Instance
        {
            get
            {
                if (!instance)
                {
                    instance = Resources.Load<ShopItemsCollection>("SO_ShopItemsCollection");
                    instance.Initialize();
                }

                return instance;
            }
        }

        private void Initialize()
        {
            shopItemsDict = new();
            foreach (ShopItem item in shopItems)
            {
                shopItemsDict.Add(item.ID, item);
            }
        }

        public ShopItem GetBoatItem()
        {
            string itemID = SaveManager.Instance.Save.BoatItemInUse;
            if (shopItemsDict.ContainsKey(itemID))
            {
                return shopItemsDict[itemID];
            }
            
            return null;
        }
        
        public ShopItem GetLineItem()
        {
            string itemID = SaveManager.Instance.Save.LineItem;
            if (shopItemsDict.ContainsKey(itemID))
            {
                return shopItemsDict[itemID];
            }
            
            return null;
        }
    }
}