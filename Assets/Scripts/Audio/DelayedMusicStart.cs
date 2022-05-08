using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedMusicStart : MonoBehaviour
{
    [SerializeField] AudioSource music;
    [SerializeField] float delay;

    WaitForSeconds audioDelay;


    void Start()
    {
        audioDelay = new WaitForSeconds(delay);
        StartCoroutine(StartAfterDelay());
    }

    IEnumerator StartAfterDelay()
    {
        yield return audioDelay;
        music.Play();
    }
}
