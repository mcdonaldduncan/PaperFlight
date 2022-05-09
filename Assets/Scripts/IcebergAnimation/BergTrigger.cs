using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BergTrigger : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;
    [SerializeField] private AudioSource glacierAudio;
    [SerializeField] float delayTime;

    private WaitForSeconds delay;
    Timer timer;

    void Start()
    {
        timer = GetComponent<Timer>();
        timer.InitializeTimer(delayTime, "BergAnimation");
        //delay = new WaitForSeconds(delayTime);
        timer.OnTimerEnd += OnTimerEnd; // bind delegate
        myAnimator.SetBool("Berg", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer.StartTimer();
           // StartCoroutine(AudioAfterDelay());
            myAnimator.SetBool("Berg", true);
        }
        
    }

    //IEnumerator AudioAfterDelay()
    //{
    //    yield return delay;
    //}

    void OnTimerEnd()
    {
        glacierAudio.Play();
    }
}
