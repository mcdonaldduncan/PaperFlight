using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] GameObject meteorSequence;
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] float speed;

    bool shouldMove;
    Vector3 trajectory;

    void Start()
    {
        shouldMove = false;
        transform.position = start.position;
    }

    void Update()
    {
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
}
