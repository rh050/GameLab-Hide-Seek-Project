using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject characterSelectionPanel;

    void Start()
    {
      //  if (SceneManager.GetActiveScene().name != "MainMenu")
      //  {
      //      Destroy(gameObject);
       // }
        characterSelectionPanel.SetActive(false); // Hide panel initially
    }

    public void ShowCharacterSelection()
    {
        characterSelectionPanel.SetActive(true); // Show selection panel
    }

    public void StartGame()
    {
        if (characterSelectionPanel.activeSelf) // Prevent game from starting if the panel is open
        {
            Debug.Log("Please select a character before starting the game.");
            return;
        }

        SceneManager.LoadScene("Game_one"); // Load the game only after confirming character selection
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
