using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopUpgradeMenu", menuName = "New Shop Upgrade")]
public class ShopUpgradeSO : ScriptableObject
{
    [field: SerializeField]
    public string Title { get; private set; }
    
    [field: SerializeField]
    public Sprite Sprite { get; private set; }
    
    [field: SerializeField]
    public int BasePrice { get; private set; }
    
    [field: SerializeField]
    public int PriceIncrement { get;  private set; }
    
    [field: SerializeField]
    public int UpgradeIncrement { get;  private set; }
    
    public enum UpgradeType
    {
        SpeedShip,
        LineLength
    }

    [field: SerializeField] 
    public UpgradeType upgradeType;
    
    [HideInInspector]
    public int currentLevel;
    
    [HideInInspector]
    public int maxLevel = 4;
    
    public int GetCurrentPrice()
    {
        return BasePrice + (currentLevel * PriceIncrement);
    }
}