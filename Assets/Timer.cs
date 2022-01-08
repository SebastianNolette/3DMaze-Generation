using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 0;
    public bool timerIsRunning = false;
    public Text timeText;

    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = false;
    }

    void Update()
    {

        float toggle = Input.GetAxisRaw("TimerToggle");

        if (toggle > 0)
        {
            timerIsRunning = true;
            toggle = 0;
        }
        if (toggle < 0)
        {
            timerIsRunning = false;
            toggle = 0;
        }
        if (timerIsRunning == true)
        {
            timeRemaining += Time.deltaTime;
        }
        DisplayTime(timeRemaining);

    }

    void DisplayTime(float timeToDisplay)
    {

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}