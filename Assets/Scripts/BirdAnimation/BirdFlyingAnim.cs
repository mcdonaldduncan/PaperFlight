using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFlyingAnim : MonoBehaviour
{
    Bounds ColBounds;
    public bool Triggered = false;
    [SerializeField] Collider Col;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Animator myAnimator2;
    [SerializeField] private Animator myAnimator3;
    [SerializeField] private Animator myAnimator4;
    [SerializeField] Transform plane;
    // Start is called before the first frame update
    void Start()
    {
        Triggered = false;
        ColBounds = Col.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (ColBounds.Contains(plane.position))
            myAnimator.SetBool("Bird", true);
            myAnimator2.SetBool("Bird2", true);
            myAnimator3.SetBool("Bird3", true);
            myAnimator4.SetBool("Bird4", true);

        Triggered = true;
    }
}
