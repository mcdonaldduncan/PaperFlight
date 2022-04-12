using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePathfinder : MonoBehaviour
{
    [SerializeField] Transform[] nodes;
    [SerializeField] Transform plane;
    [SerializeField] Collider col;
    [SerializeField] float speed;
    [SerializeField] int index;

    [System.NonSerialized] public bool shouldDestroy;
    [System.NonSerialized] public bool shouldMove;

    Bounds colBounds;

    void Start()
    {
        index = 0;
        shouldDestroy = false;
        shouldMove = false;
        colBounds = col.bounds;
    }

    void Update()
    {
        CheckStart();
        if (shouldMove)
            FollowNodes();
    }

    void FollowNodes()
    {
        if (Vector3.Distance(transform.position, nodes[index].position) < 1f)
            index++;
        if (index == nodes.Length)
        {
            shouldDestroy = true;
            return;
        }
            

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, nodes[index].position, step);
    }

    void CheckStart()
    {
        if (colBounds.Contains(plane.position))
            shouldMove = true;
    }
}
