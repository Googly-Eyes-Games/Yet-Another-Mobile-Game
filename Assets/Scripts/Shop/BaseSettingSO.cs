using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseSettingsSO", menuName = "Base settings")]
public class BaseSettingSO : ScriptableObject
{
    [field: SerializeField]
    public float ShipSpeedLevel { get; private set; }
    
    [field: SerializeField]
    public float LineLengthLevel { get; private set; }
    
    [field: SerializeField]
    public Sprite Sprite { get; private set; }
    
    [field: SerializeField]
    public Color LineColor { get; private set; }
}
