using UnityEngine;

public class OilSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject oilTilePrefab;
    
    [SerializeField]
    private float oilTileSize = 1f;
    
    [SerializeField]
    private float oilTileNoiseOffset = 0.1f;

    [SerializeField]
    private Vector2 spawnerSize;
    
    private Vector3 lastSpawnPosition;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnerSize);
    }

    void Start()
    {
        lastSpawnPosition = transform.position;
    }

    void Update()
    {
        float distanceToLastSpawn = Vector3.Distance(lastSpawnPosition, transform.position);
        if (distanceToLastSpawn > oilTileSize)
        {
            Debug.Log("Spawn");
            SpawnRow();
            lastSpawnPosition = transform.position;
        }
    }

    void SpawnRow()
    {
        int tilesNumber = Mathf.FloorToInt(spawnerSize.x / oilTileSize);
        Vector3 tilesStart = transform.position + Vector3.left * (spawnerSize.x / 2f);
        tilesStart += Vector3.right * (0.5f * oilTileSize);
        
        for (int tileId = 0; tileId < tilesNumber; tileId++)
        {
            Vector3 tilePosition = tilesStart + tileId * oilTileSize * Vector3.right;
            
            // TODO: pooling
            GameObject newOilTile = Instantiate(oilTilePrefab, tilePosition, Quaternion.identity);
        }
    }
}
