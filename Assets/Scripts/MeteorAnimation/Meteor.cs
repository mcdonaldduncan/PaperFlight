using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] Transform plane;
    [SerializeField] GameObject meteorSequence;
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] float speed;
    [SerializeField] Collider col;

    bool shouldMove;
    Bounds colBounds;

    void Start()
    {
        shouldMove = false;
        colBounds = col.bounds;
        transform.position = start.position;
    }

    void Update()
    {
        CheckStart();
        if (shouldMove)
            Launch();

    }

    void Launch()
    {
        transform.rotation = Quaternion.LookRotation(end.position - transform.position);

        if (Vector3.Distance(transform.position, end.position) < 1f)
            Destroy(meteorSequence);

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, end.position, step);
    }

    void CheckStart()
    {
        if (shouldMove)
            return;
        if (colBounds.Contains(plane.position))
            shouldMove = true;
    }
}
