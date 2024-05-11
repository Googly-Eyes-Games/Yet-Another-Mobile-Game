using UnityEngine;

namespace DefaultNamespace
{
    public class ColorPalette : ScriptableObject
    {
        [field: SerializeField]
        public Color ForeGroundUnselected { get; private set; }
        
        [field: SerializeField]
        public Color ForeGroundSelected { get; private set; }
        
        [field: SerializeField]
        public Color WaterTile { get; private set; }
        
        [field: SerializeField]
        public Color OilTile { get; private set; }
        
        [field: SerializeField]
        public Color LandTile { get; private set; }

        private static ColorPalette instance;

        public static ColorPalette Get()
        {
            if (!instance)
            {
                instance = Resources.Load<ColorPalette>("SO_ColorPalette");
            }

            return instance;
        }
    }
}