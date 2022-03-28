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
        //FollowTarget();
        ThirdPersonPosition();
    }


    void FollowTarget()
    {
        //Vector3 targetRotation = target.rotation.eulerAngles;

        transform.position = new Vector3(target.position.x + xOffset, target.position.y + yOffset, target.position.z + zOffset);
        //transform.rotation = Quaternion.Euler(transform.rotation.x, targetRotation.x, transform.rotation.z);
        
    }
    
    void FollowRotation()
    {
        float step = rotationDelta * Time.deltaTime;

        Vector3 eulerTarget = Vector3.MoveTowards(transform.rotation.eulerAngles, target.rotation.eulerAngles, step);
        Quaternion targetRotation = Quaternion.Euler(eulerTarget);

        transform.rotation = targetRotation;
    }

    // calculate polar coordinates behind target object
    void ThirdPersonPosition()
    {

        float x = (xOffset) * Mathf.Sin(target.rotation.y);
        float z = (xOffset) * Mathf.Cos(target.rotation.y);

        Vector3 targetPosition = new Vector3(x, yOffset, z);

        transform.position = target.position + targetPosition;
    }

}
