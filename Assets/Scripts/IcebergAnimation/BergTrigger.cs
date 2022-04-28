using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BergTrigger : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;

    void Start()
    {
        myAnimator.SetBool("Berg", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myAnimator.SetBool("Berg", true);
        }
        
    }
}
