using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public struct TimerHandle
    {
        public bool timerIsRunning;
        public float timeRemaining;
        public string name;
    } 

    public delegate void OnTimerEndHandler();
    public event OnTimerEndHandler OnTimerEnd;

    public TimerHandle currentHandle;

    public void InitializeTimer(float duration, string name)
    {
        currentHandle = new TimerHandle();
        currentHandle.timeRemaining = duration;
        currentHandle.name = name;
    }

    private void Update()
    {
        if (currentHandle.timerIsRunning)
        {
            RunTimer();
        }
    }

    public void StartTimer()
    {
        currentHandle.timerIsRunning = true;
    }

    public void StopTimer()
    {
        currentHandle.timeRemaining = 0;
        currentHandle.timerIsRunning = false;
        OnTimerEnd();
    }

    private void RunTimer()
    {
        if (currentHandle.timeRemaining > 0)
        {
            currentHandle.timeRemaining -= Time.deltaTime;
        }
        else
        {
            StopTimer();
        }
    }
}
