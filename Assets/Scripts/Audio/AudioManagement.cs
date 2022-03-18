using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    [SerializeField] AudioSource caveAudio;
    [SerializeField] AudioSource baseMelody;
    [SerializeField] AudioSource baseStrings;
    [SerializeField] Collider caveCollider;

    Bounds bounds;

    void Start()
    {
        bounds = caveCollider.bounds;
    }

    void Update()
    {
        CheckAudioStart();
        CheckAudioEnd();
    }

    void CheckAudioStart()
    {
        if (caveAudio.isPlaying)
            return;

        if (bounds.Contains(transform.position))
        {
            caveAudio.Play();
            baseStrings.Play();
            baseMelody.Stop();
        }
    }

    void CheckAudioEnd()
    {
        if (!caveAudio.isPlaying)
            return;

        if (!bounds.Contains(transform.position))
        {
            caveAudio.Stop();
            baseStrings.Play();
            baseMelody.Play();
        }
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Collision Enter");
    //    if (!caveAudio.isPlaying)
    //    {
    //        caveAudio.Play();
    //        baseMelody.Stop();
    //    }
    //}


    //void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log("Collision Exit");
    //    if (caveAudio.isPlaying)
    //    {
    //        caveAudio.Stop();
    //        baseMelody.Play();
    //    }
    //}
}
