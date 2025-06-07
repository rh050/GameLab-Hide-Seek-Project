using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject characterSelectionPanel;

    public GameObject mainMenuPanel;
    public GameObject difficultyPanel;
    public GameObject controlsPanel;

    void Start()
    {

    }

    public void ShowCharacterSelection()
    {
        if (characterSelectionPanel == null)
        {
            Debug.LogError("characterSelectionPanel is missing! Cannot show selection panel.    ");
            return;
        }

        characterSelectionPanel.SetActive(true);
    }

    public void StartGame()
    {
        characterSelectionPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void BackFromCharacterToMenu()
    {
        characterSelectionPanel.SetActive(false);
        difficultyPanel.SetActive(false);
    }

    public void BackFromDifficultyToCharacter()
    {
        difficultyPanel.SetActive(false);
        characterSelectionPanel.SetActive(true);
        
    }
    public void ToggleControlsPanel()
    {
        bool isActive = controlsPanel.activeSelf;
        controlsPanel.SetActive(!isActive);
        mainMenuPanel.SetActive(isActive); 
    }


}
