using UnityEngine;
using TMPro;

public class GameHUDController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI hidersLeftText;

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
}
