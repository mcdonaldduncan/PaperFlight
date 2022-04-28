using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManagement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] AudioSource windAudio;
    [SerializeField] float fadeSpeed;

    float fullVolume = 1;
    float minVolume = 0;

    void Start()
    {
        windAudio.volume = fullVolume;
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cave"))
        {
            StartCoroutine(FadeAudio(windAudio, minVolume));
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cave"))
        {
            StartCoroutine(FadeAudio(windAudio, fullVolume));
        }
    }

    IEnumerator FadeAudio(AudioSource audioToAdjust, float volume)
    {
        while (audioToAdjust.volume != volume)
        {
            float step = fadeSpeed * Time.deltaTime;
            audioToAdjust.volume = Mathf.MoveTowards(audioToAdjust.volume, volume, step);
            yield return null;
        }

        yield break;
    }
}
