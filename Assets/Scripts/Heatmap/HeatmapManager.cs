using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapManager : MonoBehaviour
{
    public static HeatmapManager Instance;
    public GameObject redZoneMarkerPrefab;
    private HashSet<Hider> trackingHiders = new HashSet<Hider>();
    private HashSet<Vector2> redZones = new HashSet<Vector2>();
    private Dictionary<Vector2, GameObject> redZoneMarkers = new Dictionary<Vector2, GameObject>(); 
    private bool isInUse = false;
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
        if (trackingHiders.Contains(hider)) 
            return;

        trackingHiders.Add(hider);
        TrackRedZone(hider);
    }

    public void TrackRedZone(Hider hider)
    {
            Vector2 startPos = hider.transform.position;
            waitFunction(1.5f);
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
    
    private IEnumerator waitFunction(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isInUse = false;
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
