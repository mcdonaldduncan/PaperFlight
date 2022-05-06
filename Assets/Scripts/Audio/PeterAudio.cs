using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeterAudio : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] AudioSource pianoHarmony;
    [SerializeField] AudioSource stringsEnsemble;
    [SerializeField] AudioSource pianoSparks;
    [SerializeField] AudioSource fluteMelody;
    [SerializeField] AudioSource bassoonMelody;
    [SerializeField] AudioSource hornEnsemble;
    [SerializeField] Collider caveCollider;
    [SerializeField] Collider lakeCollider;
    [SerializeField] Collider mountainCollider;
    [SerializeField] Collider birdCollider;
    [SerializeField] float fadeSpeed;

    float fullVolume = 1;
    float minVolume = 0;

    Bounds caveBounds;
    Bounds lakeBounds;
    Bounds mountainBounds;
    Bounds birdBounds;

    void Start()
    {
        caveBounds = caveCollider.bounds;
        mountainBounds = mountainCollider.bounds;
        lakeBounds = lakeCollider.bounds;
        birdBounds = birdCollider.bounds;

        hornEnsemble.volume = minVolume;
        bassoonMelody.volume = minVolume;
        fluteMelody.volume = minVolume;
        pianoSparks.volume = minVolume;
        pianoHarmony.volume = fullVolume;
        stringsEnsemble.volume = fullVolume;
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }

    void CheckCaveStart()
    {
        if (pianoSparks.volume != minVolume)
            return;
        if (caveBounds.Contains(transform.position))
        {
            StartCoroutine(FadeAudio(pianoSparks, fullVolume));
            StartCoroutine(FadeAudio(stringsEnsemble, minVolume));
            StartCoroutine(FadeAudio(fluteMelody, fullVolume));
        }
    }

    void CheckLakeStart()
    {
        if (stringsEnsemble.volume != minVolume)
            return;
        if (lakeBounds.Contains(transform.position))
        {
            StartCoroutine(FadeAudio(fluteMelody, minVolume));
            StartCoroutine(FadeAudio(stringsEnsemble, fullVolume));
        }
    }

    void CheckBirdStart()
    {
        if (bassoonMelody.volume != minVolume)
            return;
        if (birdBounds.Contains(transform.position))
        {
            StartCoroutine(FadeAudio(stringsEnsemble, minVolume));
            StartCoroutine(FadeAudio(bassoonMelody, fullVolume));
        }
    }

    void CheckMountainStart()
    {
        if (mountainBounds.Contains(transform.position))
        {
            if (hornEnsemble.volume != minVolume)
                return;
            StartCoroutine(FadeAudio(hornEnsemble, fullVolume));
            StartCoroutine(FadeAudio(stringsEnsemble, fullVolume));
            StartCoroutine(FadeAudio(pianoSparks, minVolume));
            StartCoroutine(FadeAudio(pianoHarmony, minVolume));
            StartCoroutine(FadeAudio(bassoonMelody, minVolume));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cave"))
        {
            StartCoroutine(FadeAudio(pianoSparks, fullVolume));
            StartCoroutine(FadeAudio(stringsEnsemble, minVolume));
            StartCoroutine(FadeAudio(fluteMelody, fullVolume));
        }
        else if (other.CompareTag("Lake"))
        {
            StartCoroutine(FadeAudio(fluteMelody, minVolume));
            StartCoroutine(FadeAudio(stringsEnsemble, fullVolume));
        }
        else if (other.CompareTag("Birds"))
        {
            StartCoroutine(FadeAudio(stringsEnsemble, minVolume));
            StartCoroutine(FadeAudio(bassoonMelody, fullVolume));
        }
        else if (other.CompareTag("Mountain"))
        {
            StartCoroutine(FadeAudio(hornEnsemble, fullVolume));
            StartCoroutine(FadeAudio(stringsEnsemble, fullVolume));
            StartCoroutine(FadeAudio(pianoSparks, minVolume));
            StartCoroutine(FadeAudio(pianoHarmony, minVolume));
            StartCoroutine(FadeAudio(bassoonMelody, minVolume));
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
