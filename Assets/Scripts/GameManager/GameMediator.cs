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
    public int SeekerUpgardeStart = 90;
    public bool upgradeSpeed = false;
    public bool upgradeXray = false;
    private bool playonce = false;
 	private bool gameStarted = false;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
 			DontDestroyOnLoad(gameObject);
        }
    }

	private void Start()
   	{
        //hud.PrepareForGame(); 
    }
    
    private void Update()
    {
        // if (!gameStarted && !hud.IsCountingDown) 
        // {
        //     StartGame();
        // }

        if (gameStarted && GameTime > 0) 
        {
            GameTime -= Time.deltaTime;

            if (GameTime <= SeekerUpgardeStart && GameTime > 0 && seekerPoints < 20 && !playonce)
            {
                if (upgradeSpeed)
                {
                    ActivateSeekerUpgrade("speed");
                }
                if (upgradeXray)
                {
                    ActivateSeekerUpgrade("xray");
                }
                playonce = true;
            }


            if (GameTime <= 0)
            {
                AwardSurvivingHiders();
                EndGame("Time's Up! Hiders Win!");
            }
        }
    }	

    //Smart Objects
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
    
    
    
    //Hiding Spots
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
    public void NotifyHidingSpotCollapsed(HidingSpot spot)
    {
        hidingSpots.Remove(spot);
		hud.DisplayMessage("A hiding spot collapsed!",2f);
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
    
    //Heatmap
    public void RegisterHeatmapManager(HeatmapManager heatmap)
    {
        heatmapManager = heatmap;
    }
   
    public void RegisterMovement(Vector2 position)
    {
        heatmapManager.RegisterMovement(position);
        
    }

    public Vector2 GetHottestZone()
    {
        if (heatmapManager != null)
        {
            return heatmapManager.GetHottestZone();
        }
        return Vector2.zero;
    }
    
    public void DeleteHottestZone(Vector2 position)
    {
        heatmapManager.DeleteHottestZone(position);
    }
    public void DisplayHeatmap()
    {
        if (heatmapManager != null)
        {
            heatmapManager.DisplayHeatmap();
        }
    }
    public List<Hider> GetAllHiders()
    {
        return hiders; 
    }

    
    //Hud functions
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
    private void UpdateHUDPoints()
    {
        if (hud != null)
        {
            hud.UpdatePoints(seekerPoints, hidersPoints);
        }
    }
    
    //Ranking
    
    public void NotifyHiderFound(Hider hider)
    {
		hud.DisplayMessage("A hider has been found!",2f);
        hiders.Remove(hider);
        AddSeekerPoints(10); 
        UpdateHidersCount();

        if (hiders.Count == 0)
        {
            EndGame("All Hiders Found! Seeker Wins!");
        }
    }
    public void NotifyHiderSpotted(Hider hider)
    {
        SeekerAI seeker = this.seeker.GetComponent<SeekerAI>();
        if (seeker != null)
        {
            seeker.SetChaseTarget(hider.transform);
            hud.DisplayMessage("Seeker is chasing a hider!", 2f);
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
    
    public void AwardSurvivingHiders()
    {
        foreach (var hider in hiders)
        {
            AddHiderPoints(5); 
        }
    }
    
    //Energy Manager
    public void UpdateEnergyHUD(float currentEnergy, float maxEnergy)
    {
        if (hud != null) 
        {
            hud.UpdateEnergyHUD(currentEnergy, maxEnergy);
        }
        else
        {
            Debug.LogWarning("HUD is not assigned to GameMediator!");
        }
    }
    
    //Abilities
    public void ActivateHiderUpgrade(PlayerController player, string upgrade)
    {
        if (upgrade == "invisibility")
        {
            PowerManager.Instance.ActivateInvisibility(player.gameObject, 5f);
			hud.DisplayMessage("Hider is Invisible!",2f);
        }
        else if (upgrade == "speed")
        {
            PowerManager.Instance.ActivateSpeedBoost(player.gameObject, 2f, 5f);
			hud.DisplayMessage("Hider has gained Speed Boost!",2f);
        }
    }

    public void ActivateSeekerUpgrade(string upgrade)
    {
        if (upgrade == "xray")
        {
			
            PowerManager.Instance.ActivateXRayVision(5f);
			hud.DisplayMessage("You activated X-Ray Vision!",2f);
        }
    }
    
	//Start game
	 public void StartGame()
    {
		gameStarted = true;
		hud.DisplayMessage("Start!",2f);
		foreach (Hider hider in hiders)
		{
			PlayerController playerController = hider.gameObject.GetComponent<PlayerController>();
    		if (playerController != null)
    		{
       			 playerController.enabled = true;
    		}
		}
    }
	//Disable NPC's movement while in countdown.
	public void SetNPCMovement(bool enabled)
	{
		foreach (Hider hider in hiders)
		{
       		PlayerController playerController = hider.gameObject.GetComponent<PlayerController>();
        	if (playerController != null)
        	{
            	playerController.enabled = enabled;
        	}
    	}

    	if (seeker != null)
    	{
        	SeekerAI seekerAI = seeker.GetComponent<SeekerAI>();
        	if (seekerAI != null)
        	{
            	seekerAI.enabled = enabled;
        	}
    	}
	}

    //End Game
    private void EndGame(string result)
    {
        AwardSurvivingHiders();

        Debug.Log(result);

        SceneManager.LoadScene(0);
    }

    
}