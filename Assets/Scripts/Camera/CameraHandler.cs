using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float xOffset;
    [SerializeField] float yOffset;
    [SerializeField] float zOffset;
    [SerializeField] float rotationDelta;

    void Start()
    {
        
    }

    void Update()
    {
        //FollowTarget();
    }

    void LateUpdate()
    {
        FollowTarget();
    }


    void FollowTarget()
    {
        

        Vector3 targetVector = target.position;
        

        transform.position = new Vector3(target.position.x + xOffset, target.position.y + yOffset, target.position.z + zOffset);
        
    }


    // Use polar coordinates
    void FollowRotation()
    {
        float step = rotationDelta * Time.deltaTime;

        Vector3 eulerTarget = Vector3.MoveTowards(transform.rotation.eulerAngles, target.rotation.eulerAngles, step);
        Quaternion targetRotation = Quaternion.Euler(eulerTarget);

        transform.rotation = targetRotation;
    }

}
