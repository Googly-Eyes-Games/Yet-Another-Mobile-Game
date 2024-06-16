using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "New Shop Item")]
public class ShopItem : ScriptableObject
{
    [field: SerializeField]
    public string ID { get; private set; }
    
    [field: SerializeField]
    public string Title { get; private set; }
    
    [field: SerializeField]
    public Sprite Sprite { get; private set; }
    
    [field: SerializeField]
    public Material Material { get; private set; }
    
    [field: SerializeField]
    public int Price { get; private set; }
    
    [field: SerializeField]
    public Color Color { get; private set; }
    
    public enum ItemType
    {
        Boat,
        Line
    }

    [field: SerializeField] 
    public ItemType itemType;
    
}
