using UnityEngine;
using System.Collections.Generic;

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

    // הוספת מקום מחבוא לרשימה
    public void RegisterHidingSpot(HidingSpot spot)
    {
        hidingSpots.Add(spot);
    }

    // הסרת מקום מחבוא מהרשימה
    public void UnregisterHidingSpot(HidingSpot spot)
    {
        hidingSpots.Remove(spot);
    }

    // בחירת מקום מחבוא רנדומלי זמין
    public HidingSpot GetRandomAvailableSpot()
    {
        List<HidingSpot> availableSpots = hidingSpots.FindAll(spot => !spot.IsOccupied);
        if (availableSpots.Count > 0)
        {
            return availableSpots[Random.Range(0, availableSpots.Count)];
        }
        return null; // אם אין מקומות פנויים
    }
}
