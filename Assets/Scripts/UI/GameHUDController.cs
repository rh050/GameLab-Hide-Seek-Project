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
    public TextMeshProUGUI countdownText;
    private float countdownTime = 10f;
    private bool isCountingDown = true;
    public TextMeshProUGUI messageText;
    public Material dangerEffectMaterial;
    public float maxDistance = 6f;

    

    private void Start()
    {
        GameMediator.Instance.RegisterHUD(this);
        StartCoroutine(StartCountdown()); 
    }
    
    private void Update()
    {
        float gameTime = GameManager.Instance.GetGameTime();
        timerText.text = "Time Left: " + Mathf.CeilToInt(gameTime).ToString() + "s";
        
        // Update shader effect based on distance
        float distance = GameMediator.Instance.GetSeekerToPlayerDistance();
        float intensity = Mathf.Clamp01(1f - (distance / maxDistance));
        dangerEffectMaterial.SetFloat("_FullScreenInten", intensity * 0.6f);
        
    }

    private void OnDestroy()
    {
        if (dangerEffectMaterial != null)
        {
            dangerEffectMaterial.SetFloat("_FullScreenInten", 0f);
            dangerEffectMaterial.SetFloat("_VoroniSpeed", 0f);
        }
    }

    
    public bool IsCountingDown => isCountingDown;

    private IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);
        GameManager.Instance.SetNPCMovement(false); 

        while (countdownTime > 0)
        {
            countdownText.text = Mathf.CeilToInt(countdownTime).ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownText.gameObject.SetActive(false);
        isCountingDown = false;
        GameManager.Instance.SetNPCMovement(true); 
        GameManager.Instance.StartGame();
    }

    public void DisplayMessage(string message, float duration) 
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
    

    public void UpdateHidersLeft(int count)
    {
        hidersLeftText.text = "Hiders Left: " + count.ToString();
    }

    public void UpdatePoints(int seekerPoints, int hidersPoints)                
    {
        seekerPointsText.text = $"Seeker: {seekerPoints}";
        hidersPointsText.text = $"Hiders: {hidersPoints}";
    }


    public void UpdateEnergyHUD(float currentEnergy, float maxEnergy)
    {
        energyValueText.text = $"{currentEnergy}/{maxEnergy}";

    }
}
