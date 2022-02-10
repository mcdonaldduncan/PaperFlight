﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePathing : MonoBehaviour
{
    [SerializeField] List<Transform> nodes = new List<Transform>();
    [SerializeField] GameObject paperPlane;
    [SerializeField] float speed;

    int nodeIndex;

    void Start()
    {
        nodeIndex = 0;
    }

    void Update()
    {
        

    }

    void FixedUpdate()
    {
        if (nodeIndex == nodes.Count)
        {
            return;
        }

        float step = speed * Time.deltaTime;
        paperPlane.transform.position = Vector3.MoveTowards(paperPlane.transform.position, nodes[nodeIndex].transform.position, step);
        paperPlane.transform.right = -Vector3.MoveTowards(paperPlane.transform.position, nodes[nodeIndex].transform.position, step);


        if (Vector3.Distance(paperPlane.transform.position, nodes[nodeIndex].transform.position) < .001f)
        {
            nodeIndex++;
        }
    }
}
