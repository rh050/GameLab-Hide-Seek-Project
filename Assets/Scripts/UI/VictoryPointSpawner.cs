using UnityEngine;

public class VictoryPointSpawner : MonoBehaviour
{
    public GameObject victoryPointPrefab;
    public Transform[] spawnLocations;

    void Start()
    {
        
        SpawnVictoryPoint();
    }

    void SpawnVictoryPoint()
    {
        if (spawnLocations.Length == 0) return;

        Transform randomLocation = spawnLocations[Random.Range(0, spawnLocations.Length)];
        Instantiate(victoryPointPrefab, randomLocation.position, Quaternion.identity);
      
    }
}