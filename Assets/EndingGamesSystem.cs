using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingGamesSystem : MonoBehaviour
{
    // Function to load the Main Menu scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the exact name of your main menu scene
    }

    // Function to load the Game scene
    public void LoadGameOne()
    {
        SceneManager.LoadScene("Game_One"); // Replace "Game_One" with the exact name of your game scene
    }
}