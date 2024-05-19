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

    [Header("Noise")]
    [SerializeField]
    private float noiseScale = 0.1f;
    
    [SerializeField]
    private float noisePower = 2f;
    
    [SerializeField]
    private float noiseThreshold = 0.8f;
    
    private Vector3 lastSpawnPosition;
    
    private Vector2 seedOffset = Vector3.zero;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnerSize);
    }

    void Start()
    {
        lastSpawnPosition = transform.position;
        
        // seedOffset.x = Random.value * 2137f;
        // seedOffset.y = Random.value * 2137f;
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
            tilePosition += (MathUtils.RandomDirection() * oilTileNoiseOffset).ToVector3();

            Vector2 noiseUV = (seedOffset + tilePosition.ToVector2()) * noiseScale;
            
            float noise = Mathf.PerlinNoise(noiseUV.x, noiseUV.y);

            if (noise > noiseThreshold)
            {
                GameObject newOilTile = Instantiate(oilTilePrefab, tilePosition, Quaternion.identity);
            }
        }
    }
}
