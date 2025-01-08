using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameHUDController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI hidersLeftText;
    public TextMeshProUGUI seekerPointsText;
    public TextMeshProUGUI hidersPointsText;
    public Slider energySlider;
    public TextMeshProUGUI energyValueText;
    public TextMeshProUGUI countdownText; // טקסט חדש לספירה לאחור
    private float countdownTime = 10f; // זמן ספירה לאחור
    private bool isCountingDown = true;
    
    public TextMeshProUGUI messageText; // Add this field for the message text

    private void Start()
    {
        GameMediator.Instance.RegisterHUD(this);
        StartCoroutine(StartCountdown()); // הפעלת הספירה לאחור
    }

    public bool IsCountingDown
    {
        get { return isCountingDown; }
    }

    private IEnumerator StartCountdown()
{
    countdownText.gameObject.SetActive(true);
    GameMediator.Instance.SetNPCMovement(false); // Disable NPC movement

    while (countdownTime > 0)
    {
        countdownText.text = Mathf.CeilToInt(countdownTime).ToString();
        yield return new WaitForSeconds(1f);
        countdownTime--;
    }

    countdownText.gameObject.SetActive(false);
    isCountingDown = false;
    GameMediator.Instance.SetNPCMovement(true); // Enable NPC movement
    GameMediator.Instance.StartGame();
}

    public void DisplayMessage(string message, float duration) // פונקציה להצגת הודעות
    {
        StartCoroutine(ShowMessageCoroutine(message, duration));
    }

    private IEnumerator ShowMessageCoroutine(string message, float duration)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        messageText.gameObject.SetActive(false);
    }

    public void PrepareForGame(float preparationTime = 3f)
    {
        StartCoroutine(PreparationCoroutine(preparationTime));
    }

    private IEnumerator PreparationCoroutine(float preparationTime)
    {
        messageText.text = "Get Ready!";
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(preparationTime);
        messageText.gameObject.SetActive(false);
        StartCoroutine(StartCountdown());
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
            energyValueText.text = $"{Mathf.RoundToInt(currentEnergy)} / {Mathf.RoundToInt(maxEnergy)}";
        }
        else
        {
            Debug.LogWarning("Energy Slider is not assigned in GameHUDController!");
        }
    }
}
