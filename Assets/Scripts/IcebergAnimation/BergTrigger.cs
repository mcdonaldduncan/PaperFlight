using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BergTrigger : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;
    [SerializeField] private AudioSource glacierAudio;
    [SerializeField] float delayTime;

    private WaitForSeconds delay;

    void Start()
    {
        delay = new WaitForSeconds(delayTime);
        myAnimator.SetBool("Berg", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(AudioAfterDelay());
            myAnimator.SetBool("Berg", true);
        }
        
    }

    IEnumerator AudioAfterDelay()
    {
        yield return delay;
        glacierAudio.Play();
    }
}
