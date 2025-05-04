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
}
