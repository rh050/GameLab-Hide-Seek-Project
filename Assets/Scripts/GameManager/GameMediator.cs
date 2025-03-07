using UnityEngine;
using System.Collections.Generic;

public class GameMediator : MonoBehaviour
{
    public static GameMediator Instance;

    private List<Hider> hiders = new List<Hider>();
    private SeekerAI seeker;
    private GameHUDController hud;

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

    void Start()
    {
        if (hud == null)
        {
            hud = FindObjectOfType<GameHUDController>();
        }
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

    public List<Hider> GetAllHiders()
    {
        return hiders;
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

    public void NotifyHiderFound(Hider hider)
    {
        hud.DisplayMessage("A hider has been found!", 2f);
        hiders.Remove(hider);
        ScoreManager.Instance.AddSeekerPoints(10);
        UpdateHidersCount();

        if (hiders.Count == 0)
        {
            hud.DisplayMessage("Seeker Wins!", 3f);
            GameManager.Instance.EndGame("All Hiders Found! Seeker Wins!");
        }
    }

    public void NotifyHiderSpotted(Hider hider)
    {
        SeekerAI seeker = GameManager.Instance.GetSeeker();
        if (seeker != null)
        {
            seeker.SetChaseTarget(hider.transform);
            hud.DisplayMessage("Seeker is chasing a hider!", 2f);
        }
    }

    public GameHUDController GetHUD() => hud;
    
}
