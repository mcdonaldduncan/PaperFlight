using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSteering : MonoBehaviour
{
    [SerializeField] Transform[] nodes;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxForce;
    [SerializeField] float maxChange;
    [SerializeField] int index;

    Vector3 velocity;
    Vector3 acceleration;

    void Start()
    {
        index = 0;
        transform.position = nodes[index].position;
        transform.rotation = Quaternion.Euler((nodes[index].position - transform.position));
    }

    void Update()
    {
        FollowNodes();
    }

    void FollowNodes()
    {
        if (index == nodes.Length)
            return;

        if (transform.position.z > nodes[index].position.z + 1f || transform.position == nodes[index].position)
            index++;

        if (index == nodes.Length)
            return;

        acceleration += Steering(index);
        velocity += acceleration;

        // Intended to slowly rotate, sorta works but stutters
        //float step = maxChange * Time.deltaTime;
        //Vector3 lookRotation = Vector3.MoveTowards(transform.position, transform.position + velocity, step);
        //transform.rotation = Quaternion.Euler(lookRotation);
        //transform.rotation = Quaternion.FromToRotation(transform.position, lookRotation);
        //transform.LookAt(lookRotation);

        transform.LookAt(transform.position + velocity);

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        //velocity.Normalize();
        //velocity *= maxSpeed;

        transform.position += velocity * Time.deltaTime;
        acceleration = Vector3.zero;

    }

    Vector3 Steering(int nodeIndex)
    {
        Vector3 desired = nodes[nodeIndex].position - transform.position;
        desired = desired.normalized;
        desired *= maxSpeed;
        Vector3 steer = desired - velocity;
        steer = steer.normalized;
        steer *= maxForce;
        return steer;
    }
}
