using System.Collections.Generic;
using UnityEngine;

public class HeatmapManager : MonoBehaviour
{
    public static HeatmapManager Instance;
    public GameObject redZoneMarkerPrefab;

    private Dictionary<Hider, float> lastRedZoneTime = new Dictionary<Hider, float>();
    public float redZoneCooldown = 5f; 

    private HashSet<Vector2> redZones = new HashSet<Vector2>();
    private Dictionary<Vector2, GameObject> redZoneMarkers = new Dictionary<Vector2, GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            Debug.Log($"[Heatmap] redZones count: {redZones.Count}");
    }

    public void RegisterRedZone(Hider hider)
    {
        float lastTime = -redZoneCooldown; 
        lastRedZoneTime.TryGetValue(hider, out lastTime);

        if (Time.time - lastTime < redZoneCooldown)
            return;

        lastRedZoneTime[hider] = Time.time;
        TrackRedZone(hider);
    }

    public void TrackRedZone(Hider hider)
    {
        Vector2 startPos = hider.transform.position;

        if (!redZones.Contains(startPos))
        {
            redZones.Add(startPos);

            if (redZoneMarkerPrefab != null && !redZoneMarkers.ContainsKey(startPos))
            {
                GameObject marker = Instantiate(
                    redZoneMarkerPrefab,
                    (Vector3)startPos,
                    Quaternion.identity
                );
                redZoneMarkers[startPos] = marker;
            }
        }
    }

    public bool HasRedZones() => redZones.Count > 0;

    public Vector2 GetNearestRedZone(Vector2 seekerPos)
    {
        Vector2 best = Vector2.zero;
        float bestDist = float.MaxValue;
        foreach (var pos in redZones)
        {
            float d = Vector2.Distance(seekerPos, pos);
            if (d < bestDist)
            {
                bestDist = d;
                best = pos;
            }
        }
        return best;
    }

    public void RemoveRedZone(Vector2 pos)
    {
        if (redZones.Remove(pos))
        {
            if (redZoneMarkers.TryGetValue(pos, out var marker) && marker != null)
            {
                Destroy(marker);
                redZoneMarkers.Remove(pos);
            }
        }
    }
}
