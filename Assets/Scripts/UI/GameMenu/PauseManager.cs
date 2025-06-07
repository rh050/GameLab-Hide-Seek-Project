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
            if (controlsPanel.activeSelf)
            {
                controlsPanel.SetActive(false);
                pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                TogglePause();
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleControlsPanel();
        }
    }

    public void ToggleControlsPanel()
    {
        bool isActive = controlsPanel.activeSelf;
        controlsPanel.SetActive(!isActive);

        if (!isActive)
        {
            pausePanel.SetActive(false);
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
