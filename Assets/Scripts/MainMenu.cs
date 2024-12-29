using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartGame();
        }
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}