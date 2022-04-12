﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBird : MonoBehaviour
{
    [SerializeField] Transform target;

    NodePathfinder nodePatfhinder;

    float rotation;
    float period;
    float offset;

    void Start()
    {
        nodePatfhinder = GameObject.Find("BirdNavigator").GetComponent<NodePathfinder>();
        rotation = Random.Range(0f, 360f);
        period = Random.Range(2f, 6f);
        offset = Random.Range(2f, 6f);
    }

    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        float oscillation = offset * Mathf.Cos((2 * Mathf.PI) * Time.time / period);

        rotation += Time.deltaTime;

        float x = oscillation * Mathf.Sin(rotation);
        float y = oscillation * Mathf.Sin(rotation);
        float z = oscillation * Mathf.Cos(rotation);
        
        Vector3 targetPosition = new Vector3(x, y, z);

        transform.position = target.position + targetPosition;
    }

    void CheckDestroy()
    {
        if (nodePatfhinder.shouldDestroy == true)
            Destroy(gameObject);
    }
}
