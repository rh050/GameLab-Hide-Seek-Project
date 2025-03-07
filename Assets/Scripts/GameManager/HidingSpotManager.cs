using System.Collections.Generic;
using UnityEngine;

public class HidingSpotManager : MonoBehaviour
{
    public static HidingSpotManager Instance;
    private List<HidingSpot> hidingSpots = new List<HidingSpot>();

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

    public void RegisterHidingSpot(HidingSpot spot)
    {
        hidingSpots.Add(spot);
    }

    public void NotifyHidingSpotCollapsed(HidingSpot spot)
    {
        hidingSpots.Remove(spot);
        GameMediator.Instance.GetHUD().DisplayMessage("A hiding spot collapsed!", 2f);
    }

    public HidingSpot GetRandomAvailableSpot()
    {
        List<HidingSpot> availableSpots = hidingSpots.FindAll(spot => !spot.IsOccupied);
        if (availableSpots.Count > 0)
        {
            return availableSpots[Random.Range(0, availableSpots.Count)];
        }
        return null;
    }
}
