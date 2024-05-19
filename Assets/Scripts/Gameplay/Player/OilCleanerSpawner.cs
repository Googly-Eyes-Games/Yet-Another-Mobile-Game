using System;
using UnityEngine;

[RequireComponent(typeof(RopeHandler))]
public class OilCleanerSpawner : MonoBehaviour
{
    // TODO: Object pooling
    [SerializeField]
    private GameObject oilCleanerPrefab;
    
    private RopeHandler ropeHandler;

    private void Awake()
    {
        ropeHandler = GetComponent<RopeHandler>();
        ropeHandler.OnCloseLoopDetected += SpawnOilCleaner;
    }

    private void SpawnOilCleaner(Vector3[] ropeLoop)
    {
        GameObject newCleaner = Instantiate(oilCleanerPrefab, transform.position, transform.rotation);
        OilCleaner cleanerComponent = newCleaner.GetComponent<OilCleaner>();
        cleanerComponent.CleanOil(ropeLoop);
    }
}
