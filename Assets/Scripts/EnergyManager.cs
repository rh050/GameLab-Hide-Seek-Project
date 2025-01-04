using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance { get; private set; } // Singleton instance

    public float maxEnergy = 30f;
    public float currentEnergy;
    public float energyRegenRate = 1f;
    public Text energyText;

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        currentEnergy = 0f; // Start with 0 energy
        InvokeRepeating(nameof(RegenerateEnergy), 1f, 1f); // Regenerate energy every second
    }

    private void RegenerateEnergy()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy = Mathf.Min(currentEnergy + energyRegenRate, maxEnergy);
            GameMediator.Instance.UpdateEnergyHUD(currentEnergy, maxEnergy); // Update the HUD
        }
    }

    public bool UseEnergy(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            GameMediator.Instance.UpdateEnergyHUD(currentEnergy, maxEnergy); // Update the HUD
            return true; // Enough energy to use
        }
        return false; // Not enough energy
    }
}