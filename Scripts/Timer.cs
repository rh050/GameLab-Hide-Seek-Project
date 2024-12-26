using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeLeft = 60f; // Initial time in seconds
    public bool timeIsRunning = true;
    public TMP_Text timeText; // Reference to the TextMeshPro UI component

    void Start()
    {
        timeIsRunning = true;
        UpdateTimeDisplay(); // Initialize the timer display
    }

    void Update()
    {
        if (timeIsRunning)
        {
            
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime; // Decrease time by the time passed since the last frame
                UpdateTimeDisplay(); // Update the displayed time
                
            }
            else
            {
                timeLeft = 0;
                timeIsRunning = false; // Stop the timer
                UpdateTimeDisplay();
                OnTimerEnd(); // Call a method when the timer ends
            }
        }
    }

    void UpdateTimeDisplay()
    {
        // Format the time as minutes:seconds and update the TMP_Text component
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnTimerEnd()
    {
        // Add any logic here that should happen when the timer reaches 0
        Debug.Log("Timer has ended!");
        SceneManager.LoadScene("MainMenu");
       
    }
}
