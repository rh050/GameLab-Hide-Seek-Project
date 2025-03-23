using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI resultText; 
    public Button continueButton;     

    void Start()
    {
        
        if (resultText != null)
        {
            resultText.text = GameData.GameResult;
        }
        else
        {
            Debug.LogWarning("Result Text is not assigned in GameOverManager!");
        }

       
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
        }
        else
        {
            Debug.LogWarning("Continue Button is not assigned in GameOverManager!");
        }
    }


    private void OnContinueClicked()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
}