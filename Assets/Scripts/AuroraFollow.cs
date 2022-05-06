using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuroraFollow : MonoBehaviour
{
    [SerializeField] Transform target;

    void Update()
    {
        if (target == null)
            return;

        if (target.position.z > transform.position.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z);
        }
    }
}
