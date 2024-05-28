using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "New Shop Item")]
public class ShopItemSO : ScriptableObject
{
    [field: SerializeField]
    public string Title { get; private set; }
    
    [field: SerializeField]
    public Sprite Sprite { get; private set; }
    
    [field: SerializeField]
    public int Price { get; private set; }
    
    [field: SerializeField]
    public Color Color { get; private set; }
    
}
