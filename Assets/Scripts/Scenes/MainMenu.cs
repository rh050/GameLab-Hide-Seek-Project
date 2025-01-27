using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            Destroy(gameObject);
        }
    }
    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartGame();
        }
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("Game_one");
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        
    }
}