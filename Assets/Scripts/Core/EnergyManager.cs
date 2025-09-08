using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance { get; private set; } 

    public float maxEnergy = 30f;
    public float currentEnergy;
    public float energyRegenRate = 1f;
    public Text energyText;
    private GameHUDController hud; 

    private void Awake()
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

    private void Start()
    {
        currentEnergy = 0f; 
        hud = FindObjectOfType<GameHUDController>(); 
        InvokeRepeating(nameof(RegenerateEnergy), 1f, 1f); 
    }

    private void RegenerateEnergy()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy = Mathf.Min(currentEnergy + energyRegenRate, maxEnergy);
            UpdateEnergyHUD();
        }
    }

    public bool UseEnergy(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            UpdateEnergyHUD();
            return true; 
        }
        return false; 
    }

    private void UpdateEnergyHUD()
    {
        if (hud != null)
        {
            hud.UpdateEnergyHUD(currentEnergy, maxEnergy);
        }
    }
}
