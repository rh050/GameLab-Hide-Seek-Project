using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject characterSelectionPanel;

    void Start()
    {

    }

    public void ShowCharacterSelection()
    {
        if (characterSelectionPanel == null)
        {
            Debug.LogError("characterSelectionPanel is missing! Cannot show selection panel.");
            return;
        }

        characterSelectionPanel.SetActive(true);
    }

    public void StartGame()
    {
        if (characterSelectionPanel != null && characterSelectionPanel.activeSelf)
        {
            Debug.Log("Please select a character before starting the game.");
            return;
        }

        SceneManager.LoadScene("Game_one");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
