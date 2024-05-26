using Gameplay.OilSpill;
using UnityEngine;

public class OilSpawner : MonoBehaviour
{
    [SerializeField]
    private AvailableOilSpillsPrefabs spillsPrefabs;
    
    [SerializeField]
    private Vector2 spawnerSize;
    
    [SerializeField]
    private float distanceToSpawn;

    private Vector3 lastSpawnPosition;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnerSize);
    }

    void Start()
    {
        lastSpawnPosition = transform.position - Vector3.down * distanceToSpawn;
    }

    void Update()
    {
        float distanceToLastSpawn = Vector3.Distance(lastSpawnPosition, transform.position);
        if (distanceToLastSpawn > distanceToSpawn)
        {
            SpawnSpill();
            lastSpawnPosition = transform.position;
        }
    }

    private void SpawnSpill()
    {
        GameObject prefab = spillsPrefabs.GetRandom();

        GameObject newSpill = Instantiate(prefab);
        
        float halfSize = spawnerSize.x / 2f;
        newSpill.transform.position = transform.position + Vector3.right * Random.Range(-halfSize, halfSize);
        newSpill.transform.rotation = MathUtils.RandomRotation2D();
    }
}
