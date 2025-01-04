using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameMediator : MonoBehaviour
{
    public static GameMediator Instance;

    public float GameTime { get; set; } = 100f; 
    private List<HidingSpot> hidingSpots = new List<HidingSpot>();
    private List<Hider> hiders = new List<Hider>();
    private SeekerAI seeker;
    private GameHUDController hud;
    private HeatmapManager heatmapManager;
    private EnergyManager energyManager;
    private List<SmartObject> smartObjects = new List<SmartObject>();
    
    private int seekerPoints = 0;
    private int hidersPoints = 0;


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
    
    private void Update()
    {
        if (GameTime > 0)
        {
            GameTime -= Time.deltaTime;

            if (GameTime <= 0)
            {
                EndGame("Time's Up! Hiders Win!");//need to change to ui
            }
        }
    }
    
    

    public void ActivateSmartObjects(GameObject player)
    {
        foreach (SmartObject smartObject in smartObjects)
        {
            if (Vector3.Distance(player.transform.position, smartObject.transform.position) < 1.5f)
            {
                smartObject.Activate(player);
            }
        }
    }
    public void RegisterSmartObject(SmartObject smartObject)
    {
        smartObjects.Add(smartObject);
    }
    
    public void EndGame(string result)
    {
        Debug.Log(result);
        SceneManager.LoadScene(0);
    }
    public void RegisterHidingSpot(HidingSpot spot)
    {
        hidingSpots.Add(spot);
    }

    public void RegisterHider(Hider hider)
    {
        hiders.Add(hider);
        UpdateHidersCount();
        
    }

    public void RegisterSeeker(SeekerAI seekerAI)
    {
        seeker = seekerAI;
    }

    public void RegisterHeatmapManager(HeatmapManager heatmap)
    {
        heatmapManager = heatmap;
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

    public void NotifyHiderFound(Hider hider)
    {
        hiders.Remove(hider);
        AddSeekerPoints(10); 
        UpdateHidersCount();

        if (hiders.Count == 0)
        {
            EndGame("All Hiders Found! Seeker Wins!");
        }
    }

    public void NotifyHidingSpotCollapsed(HidingSpot spot)
    {
        hidingSpots.Remove(spot);
        Debug.Log("A hiding spot collapsed!");
    }

    public void RegisterMovement(Vector2 position)
    {
        if (heatmapManager != null)
        {
            heatmapManager.RegisterMovement(position);
        }
    }

    public Vector2 GetHottestZone()
    {
        if (heatmapManager != null)
        {
            return heatmapManager.GetHottestZone();
        }
        return Vector2.zero;
    }
    
    public void RegisterHUD(GameHUDController hudController)
    {
        hud = hudController;
    }

    private void UpdateHidersCount()
    {
        if (hud != null)
        {
            hud.UpdateHidersLeft(hiders.Count);
        }
    }
    public void AddSeekerPoints(int points)
    {
        seekerPoints += points;
        UpdateHUDPoints();
    }

    public void AddHiderPoints(int points)
    {
        hidersPoints += points;
        UpdateHUDPoints();
    }

    private void UpdateHUDPoints()
    {
        if (hud != null)
        {
            hud.UpdatePoints(seekerPoints, hidersPoints);
        }
    }
    public void AwardSurvivingHiders()
    {
        foreach (var hider in hiders)
        {
            AddHiderPoints(5); // נקודות למתחבאים
        }
    }
    public void UpdateEnergyHUD(float currentEnergy, float maxEnergy)
    {
        if (hud != null) // Ensure the `hud` instance is assigned
        {
            hud.UpdateEnergyHUD(currentEnergy, maxEnergy);
        }
        else
        {
            Debug.LogWarning("HUD is not assigned to GameMediator!");
        }
    }
}
