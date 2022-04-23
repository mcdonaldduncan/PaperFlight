using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] AudioSource caveAudio;
    [SerializeField] AudioSource baseMelody;
    [SerializeField] AudioSource baseStrings;
    [SerializeField] Collider caveCollider;
    [SerializeField] float fadeSpeed;

    float fullVolume = 1;
    float minVolume = 0;

    Bounds bounds;

    void Start()
    {
        bounds = caveCollider.bounds;

        baseStrings.volume = fullVolume;
        caveAudio.volume = minVolume;
        baseMelody.volume = fullVolume;
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }

        CheckAudioStart();
        CheckAudioEnd();
    }

    void CheckAudioStart()
    {
        if (caveAudio.volume != minVolume)
            return;
        if (bounds.Contains(transform.position))
        {
            StartCoroutine(FadeAudio(caveAudio, fullVolume));
            StartCoroutine(FadeAudio(baseMelody, minVolume));
        }
    }

    void CheckAudioEnd()
    {
        if (caveAudio.volume != fullVolume)
            return;
        if (!bounds.Contains(transform.position))
        {
            StartCoroutine(FadeAudio(caveAudio, minVolume));
            StartCoroutine(FadeAudio(baseMelody, fullVolume));
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
