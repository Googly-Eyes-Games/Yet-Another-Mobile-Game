using Gameplay.OilSpill;
using NaughtyAttributes;
using UnityEngine;

public class GameplaySpawner : MonoBehaviour
{
    [Foldout("Prefabs")]
    [SerializeField]
    private AvailableOilSpillsPrefabs spillsPrefabs;

    [Foldout("Prefabs")]
    [SerializeField]
    private GameObject scrapPrefab;
    
    [Foldout("Config")]
    [SerializeField]
    private Vector2 spawnerSize;
    
    [Foldout("Config")]
    [SerializeField]
    private float distanceToSpawnOil = 13;
    
    [Foldout("Config")]
    [SerializeField]
    private float spawnScrapEachXSpill = 4;

    private Vector3 lastSpawnOilPosition;

    private int spawnedSpills;
    private bool scrapSpawnedThisPass;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnerSize);
    }

    void Start()
    {
        lastSpawnOilPosition = transform.position - Vector3.down * (distanceToSpawnOil * 2f);
    }

    void Update()
    {
        float distanceToLastSpawn = Vector3.Distance(lastSpawnOilPosition, transform.position);
        if (distanceToLastSpawn > distanceToSpawnOil)
        {
            SpawnSpill();
            lastSpawnOilPosition = transform.position;
            distanceToLastSpawn = 0f;
        }
        
        if (distanceToLastSpawn > distanceToSpawnOil * 0.5f
            && spawnedSpills % spawnScrapEachXSpill == 0
            && !scrapSpawnedThisPass)
        {
            SpawnScrap();
        }
    }

    private void SpawnSpill()
    {
        GameObject prefab = spillsPrefabs.GetRandom();
        GameObject newSpill = Instantiate(prefab);

        spawnedSpills++;
        scrapSpawnedThisPass = false;
        
        RandomizePositionAndRotation(newSpill.transform);

        OilSpillMeshGenerator meshGenerator = newSpill.GetComponent<OilSpillMeshGenerator>();
        
        Vector3 boundMin = newSpill.transform.TransformPoint(meshGenerator.Bounds.min);
        Vector3 boundMax = newSpill.transform.TransformPoint(meshGenerator.Bounds.max);
        
        float halfSize = spawnerSize.x / 2f;

        if (boundMax.x > halfSize || boundMin.x > halfSize)
        {
            newSpill.transform.position -= meshGenerator.Bounds.extents;
        }
        else if (boundMax.x < -halfSize || boundMin.x < -halfSize)
        {
            newSpill.transform.position += meshGenerator.Bounds.extents;
        }
    }

    private void SpawnScrap()
    {
        GameObject newScrap = Instantiate(scrapPrefab);
        RandomizePositionAndRotation(newScrap.transform);
        scrapSpawnedThisPass = true;
    }

    private void RandomizePositionAndRotation(Transform objectTransform)
    {
        float halfSize = spawnerSize.x / 2f;
        objectTransform.position = transform.position + Vector3.right * Random.Range(-halfSize, halfSize);
        objectTransform.rotation = MathUtils.RandomRotation2D();
    }
    
}
