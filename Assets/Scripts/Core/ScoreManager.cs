using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int seekerPoints = 0;
    private int hidersPoints = 0;
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
        }
    }

    void Start()
    {
        hud = FindObjectOfType<GameHUDController>();
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
        else
        {
            Debug.LogWarning("HUD not found in ScoreManager!");
        }
    }

    public void AwardSurvivingHiders()
    {
        foreach (var hider in GameMediator.Instance.GetAllHiders())
        {
            AddHiderPoints(5);
        }
    }

    public int GetSeekerPoints() => seekerPoints;
    public int GetHiderPoints() => hidersPoints;
}
