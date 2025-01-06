using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapManager : MonoBehaviour
{
    public static HeatmapManager  Instance ;

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
        GameMediator.Instance.RegisterHeatmapManager(this);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            HeatmapManager.Instance.DebugMaxHeat();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            HeatmapManager.Instance.DisplayHeatmap();
        }
        //need to check if it's ok with luna
        GameMediator.Instance.RegisterHeatmapManager(this);


    }

    public void RegisterMovement(Vector2 position)
    {
        if (heatmapData.ContainsKey(position))
        {
            heatmapData[position]++;
        }
        else
        {
            heatmapData[position] = 1;
        }
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
    public int GetMaxHeat()
    {
        int maxHeat = 0;
        foreach (var zone in heatmapData)
        {
            if (zone.Value > maxHeat)
            {
                maxHeat = zone.Value;
            }
        }
        return maxHeat;
    }

    public void DisplayHeatmap()
    {
        if (heatmapData == null || heatmapData.Count == 0)
            return;

        int maxHeat = GetMaxHeat();
        foreach (var zone in heatmapData)
        {
            float heatRatio = (float)zone.Value / maxHeat;
            Color heatColor = Color.Lerp(Color.blue, Color.red, heatRatio);

            Debug.DrawLine(new Vector3(zone.Key.x, zone.Key.y, 0), new Vector3(zone.Key.x, zone.Key.y + 0.5f, 0), heatColor, 0.5f);
        }
    }
    public void DebugMaxHeat()
    {
        int maxHeat = GetMaxHeat();
        Debug.Log($"Max Heat: {maxHeat}");
    }


}
