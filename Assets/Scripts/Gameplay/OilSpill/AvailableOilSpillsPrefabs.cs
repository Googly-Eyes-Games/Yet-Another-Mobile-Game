using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.OilSpill
{
    [CreateAssetMenu(fileName = "SO_AvailableOilSpills", menuName = "SFG/OilSpillsConfig", order = 0)]
    public class AvailableOilSpillsPrefabs : ScriptableObject
    {
        [field: SerializeField]
        public List<GameObject> SmallOilSpills { get; private set; }
        
        [field: SerializeField]
        public List<GameObject> MediumOilSpills { get; private set; }

        public GameObject GetRandom()
        {
            List<GameObject> allPrefabs = new List<GameObject>();
            allPrefabs.AddRange(SmallOilSpills);
            allPrefabs.AddRange(MediumOilSpills);

            return allPrefabs.GetRandom();
        }
    }
}