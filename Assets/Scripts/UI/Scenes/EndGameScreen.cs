using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameScreen : MonoBehaviour
{
    [Header("UI References")]
    public Image resultImage;            
    public Sprite loseGame;      
    public Sprite winGame;       
    public Button continueButton;     

    void Start()
    {
        if (GameData.GameResult.Contains("Seeker"))
            resultImage.sprite = loseGame;
        else
            resultImage.sprite = winGame;

        resultImage.SetNativeSize();

        continueButton.onClick.AddListener(OnContinue);
    }

    private void OnContinue()
    {
        SceneManager.LoadScene("MainMenu");
    }
}