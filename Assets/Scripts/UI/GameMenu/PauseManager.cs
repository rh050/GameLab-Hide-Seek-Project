using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject controlsPanel;


    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ShowControlsPanel();
        }
    }


    public void TogglePause()
    {
        bool isActive = pausePanel.activeSelf;
        pausePanel.SetActive(!isActive);
        Time.timeScale = isActive ? 1 : 0;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu"); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ShowControlsPanel()
    {
        pausePanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void BackToPauseMenu()
    {
        controlsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }


}
