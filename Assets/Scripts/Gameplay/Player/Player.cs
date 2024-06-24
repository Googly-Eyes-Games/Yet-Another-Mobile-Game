using System;
using System.Collections;
using System.Collections.Generic;
using Shop;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    [SerializeField]
    private LineRenderer lineRenderer;
    
    [SerializeField]
    private SOEvent onSaveDataChanged;

    [SerializeField]
    private RopeHandler ropeHandler;

    public void OnEnable()
    {
        onSaveDataChanged.OnRaise += HandleSaveDataChanged;
        HandleSaveDataChanged();
    }

    public void OnDisable()
    {
        onSaveDataChanged.OnRaise -= HandleSaveDataChanged;
    }

    private void HandleSaveDataChanged()
    {
        ShopItem boatItem = ShopItemsCollection.Instance.GetBoatItem();
        if (boatItem)
        {
            spriteRenderer.sprite = boatItem.Sprite;
            spriteRenderer.color = boatItem.Color;
            spriteRenderer.transform.localScale = Vector3.one * boatItem.Scale;
        }
        
        ShopItem lineItem = ShopItemsCollection.Instance.GetLineItem();
        if (lineItem)
        {
            lineRenderer.startColor = lineItem.Color;
            lineRenderer.endColor = lineItem.Color;

            lineRenderer.material = lineItem.Material;
        }

        if (ropeHandler)
        {
            ropeHandler.maxMarksCount = ropeHandler.baseMaxMarksCount + SaveManager.Instance.Save.RopeLengthLevel;
        }
    }
}
