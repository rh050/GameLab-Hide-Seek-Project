using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameHUDController : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    public TextMeshProUGUI hidersLeftText; 

    public float gameTime = 10f; // public for now
    private int hidersLeft = 5; //need to change to real value

    void Update()
    {
        gameTime -= Time.deltaTime;
        timerText.text = "Time Left: " + Mathf.CeilToInt(gameTime).ToString() + "s";

        if (gameTime <= 0)
        {
            EndGame("Time's Up! Hiders Win!");
        }
    }

    public void UpdateHidersLeft(int count)
    {
        hidersLeft = count;
        hidersLeftText.text = "Hiders Left: " + hidersLeft.ToString();
    }

    private void EndGame(string result)
    {
        Debug.Log(result);
        SceneManager.LoadScene(0);
    }
}
