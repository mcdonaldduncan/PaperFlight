using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedMusicStart : MonoBehaviour
{
    [SerializeField] AudioSource music;
    [SerializeField] float delay;
    private Timer timer;

    void Start()
    {
        timer = GetComponent<Timer>();
        timer.InitializeTimer(delay, "AudioDelay");
        timer.StartTimer();
        timer.OnTimerEnd += OnTimerEnd; // bind delegate
    }

    private void OnTimerEnd()
    {
        music.Play();
    }
}
