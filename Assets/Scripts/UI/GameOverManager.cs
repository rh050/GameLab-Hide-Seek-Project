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

       
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
        }
    }


    private void OnContinueClicked()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
}