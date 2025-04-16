using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance { get; private set; } // Singleton instance

    public float maxEnergy = 30f;
    public float currentEnergy;
    public float energyRegenRate = 1f;
    public Text energyText;
    private GameHUDController hud; // ישירות ל-HUD במקום ה-Mediator

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // מניעת יצירת מופעים נוספים
        }
    }

    private void Start()
    {
        currentEnergy = 0f; // האנרגיה מתחילה מ-0
        hud = FindObjectOfType<GameHUDController>(); // חיפוש ה-HUD
        InvokeRepeating(nameof(RegenerateEnergy), 1f, 1f); // טעינת אנרגיה כל שנייה
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
            return true; // יש מספיק אנרגיה
        }
        return false; // אין מספיק אנרגיה
    }

    private void UpdateEnergyHUD()
    {
        if (hud != null)
        {
            hud.UpdateEnergyHUD(currentEnergy, maxEnergy);
        }
        else
        {
            Debug.LogWarning("HUD not found in EnergyManager!");
        }
    }
}
