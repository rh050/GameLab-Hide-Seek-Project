using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameHUDController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI hidersLeftText;
    public TextMeshProUGUI seekerPointsText;
    public TextMeshProUGUI hidersPointsText;
    public Slider energySlider;

    private void Start()
    {
        GameMediator.Instance.RegisterHUD(this);
    }

    private void Update()
    {
        float gameTime = GameMediator.Instance.GameTime;
        timerText.text = "Time Left: " + Mathf.CeilToInt(gameTime).ToString() + "s";

    }
    public void UpdateHidersLeft(int count)
    {
        hidersLeftText.text = "Hiders Left: " + count.ToString();
    }

    public void UpdatePoints(int seekerPoints, int hidersPoints)
    {
        seekerPointsText.text = "Seeker Points: " + seekerPoints;
        hidersPointsText.text = "Hider Points: " + hidersPoints;
    }
    public void UpdateEnergyHUD(float currentEnergy, float maxEnergy)
    {
        if (energySlider != null)
        {
            energySlider.value = currentEnergy / maxEnergy; 
        }
        else
        {
            Debug.LogWarning("Energy Slider is not assigned in GameHUDController!");
        }
    }
}