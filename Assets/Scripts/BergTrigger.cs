using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BergTrigger : MonoBehaviour
{
    Bounds colBounds;
    public bool funny = false;
    [SerializeField] Collider col;
    [SerializeField] private Animator myAnimator;
    [SerializeField] Transform plane;

    void Start()
    {
        funny = false;
        colBounds = col.bounds;
    }

    private void Update()
    {
        if (colBounds.Contains(plane.position))
            myAnimator.SetBool("Berg", true);
        funny = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            funny = true;
            myAnimator.SetBool("Berg", true);
        }
    }
}
