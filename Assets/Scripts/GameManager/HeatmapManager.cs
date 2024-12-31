using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapManager : MonoBehaviour
{

    public static HeatmapManager Instance;

    private Dictionary<Vector2, int> heatmapData = new Dictionary<Vector2, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterMovement(Vector2 position)
    {
        if (heatmapData.ContainsKey(position))
            heatmapData[position]++;
        else
            heatmapData[position] = 1;
    }

    public Vector2 GetHottestZone()
    {
        int maxHeat = 0;
        Vector2 hottestZone = Vector2.zero;

        foreach (var zone in heatmapData)
        {
            if (zone.Value > maxHeat)
            {
                maxHeat = zone.Value;
                hottestZone = zone.Key;
            }
        }

        return hottestZone;
    }
}


