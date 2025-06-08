using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HidingSpotManager : MonoBehaviour
{
    public static HidingSpotManager Instance { get; private set; }
    
    [Header("Spawner Settings")]
    public int totalHidingSpots = 20;        
    public float respawnCooldown = 10f;       
    public Tilemap levelTilemap;              
    public List<GameObject> hidingSpotPrefabs;  

    private List<HidingSpot> activeHidingSpots = new List<HidingSpot>();
    private bool isRespawning = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        SpawnHidingSpots();
    }


    private void CleanUpHidingSpots()
    {
        activeHidingSpots.RemoveAll(item => item == null);
    }

    public void RegisterHidingSpot(HidingSpot spot)
    {
        if (spot != null && !activeHidingSpots.Contains(spot))
            activeHidingSpots.Add(spot);
    }
    
    public void UnregisterHidingSpot(HidingSpot spot)
    {
        if (activeHidingSpots.Contains(spot))
            activeHidingSpots.Remove(spot);
        
 
        if (activeHidingSpots.Count < totalHidingSpots && !isRespawning)
        {
            StartCoroutine(RespawnCooldown());
        }
    }
    
public void SpawnHidingSpots()
{
    activeHidingSpots.RemoveAll(spot => spot == null);

    int missingCount = totalHidingSpots - activeHidingSpots.Count;
    if (missingCount <= 0)
        return; 

    int rows = 3;
    int cols = 3;

    Bounds localBounds = levelTilemap.localBounds;
    Vector3 worldMin = levelTilemap.transform.TransformPoint(localBounds.min);
    Vector3 worldMax = levelTilemap.transform.TransformPoint(localBounds.max);
    float tilemapWidth = worldMax.x - worldMin.x;
    float tilemapHeight = worldMax.y - worldMin.y;

    float sectionWidth = tilemapWidth / cols;
    float sectionHeight = tilemapHeight / rows;

    for (int i = 0; i < missingCount; i++)
    {
        int randomRow = Random.Range(0, rows);
        int randomCol = Random.Range(0, cols);

        Vector2 sectionOrigin = new Vector2(worldMin.x + randomCol * sectionWidth, worldMin.y + randomRow * sectionHeight);

        Vector2 randomOffset = new Vector2(Random.Range(0f, sectionWidth), Random.Range(0f, sectionHeight));
        Vector2 spawnPosition = sectionOrigin + randomOffset;

        if (IsValidPosition(spawnPosition))
        {
            GameObject prefab = hidingSpotPrefabs[Random.Range(0, hidingSpotPrefabs.Count)];
            if (prefab != null)
            {
                GameObject spotObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
                RegisterHidingSpot(spotObj.GetComponent<HidingSpot>());
            }
        }
        else
        {
            i--;
        }
    }
}



    private bool IsValidPosition(Vector2 position)
    {
        Vector3Int cellPos = levelTilemap.WorldToCell(position);
        bool hasTile = levelTilemap.HasTile(cellPos);
        Collider2D hit = Physics2D.OverlapCircle(position, 0.3f, LayerMask.GetMask("Objects"));
        return hasTile && hit == null;
    }

    private IEnumerator RespawnCooldown()
    {
        isRespawning = true;
        yield return new WaitForSeconds(respawnCooldown);
        
        CleanUpHidingSpots();
        int spotsToSpawn = totalHidingSpots - activeHidingSpots.Count;

        Bounds localBounds = levelTilemap.localBounds;
        Vector3 worldMin = levelTilemap.transform.TransformPoint(localBounds.min);
        Vector3 worldMax = levelTilemap.transform.TransformPoint(localBounds.max);

        for (int i = 0; i < spotsToSpawn; i++)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(worldMin.x, worldMax.x),
                Random.Range(worldMin.y, worldMax.y)
            );

            if (IsValidPosition(spawnPosition))
            {
                GameObject prefab = hidingSpotPrefabs[Random.Range(0, hidingSpotPrefabs.Count)];
                if (prefab != null)
                {
                    GameObject spotObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
                    RegisterHidingSpot(spotObj.GetComponent<HidingSpot>());
                }
            }
            else
            {
                i--; 
            }
        }
        isRespawning = false;
    }
    public List<HidingSpot> GetAllActiveSpots()
    {
        return new List<HidingSpot>(activeHidingSpots);
    }

}
