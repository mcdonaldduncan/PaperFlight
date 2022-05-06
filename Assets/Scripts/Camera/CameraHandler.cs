using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    // Select movement type
    [Header("Select following method")]
    [SerializeField] bool fixedOffset;
    [SerializeField] bool thirdPerson;

    // Select rotation in 3rd person
    [Header("Select if rotation is coupled in third person")]
    [SerializeField] bool coupleThirdPersonRotation;

    // Select rotation axis to follow
    [Header("Select axis to couple")]
    [SerializeField] bool coupleX;
    [SerializeField] bool coupleY;
    [SerializeField] bool coupleZ;

    // Assign target and offset values
    [Header("Assign target transform and offset values")]
    [SerializeField] Transform target;
    [SerializeField] float xOffset;
    [SerializeField] float yOffset;
    [SerializeField] float zOffset;

    // Assign smoothing value to rotation
    [Header("Assign value to smooth rotation by")]
    [SerializeField] float rotationDelta;

    float zoomXOffset;
    float zoomYOffset;
    float zoomZOffset;

    void LateUpdate()
    {
        Follow();
    }

    void Follow()
    {
        if (fixedOffset)
        {
            OffsetTargetFollow();           
        }
        if (thirdPerson)
        {
            ThirdPersonPosition();
        }
    }

    void OffsetTargetFollow()
    {
        transform.position = new Vector3(target.position.x + xOffset, target.position.y + yOffset, target.position.z + zOffset);
        FollowRotation();
    }
    
    void FollowRotation()
    {
        float step = rotationDelta * Time.deltaTime;

        Vector3 constrainedRotation = AssignRotationAxis();

        Vector3 eulerTarget = Vector3.MoveTowards(transform.rotation.eulerAngles, constrainedRotation, step);

        Quaternion targetRotation = Quaternion.Euler(eulerTarget);

        transform.rotation = targetRotation;
    }

    Vector3 AssignRotationAxis()
    {
        Vector3 eulerTarget = Vector3.zero;

        if (coupleX)
        {
            eulerTarget += new Vector3(target.rotation.eulerAngles.x, 0, 0);
        }

        if (coupleY)
        {
            eulerTarget += new Vector3(0, target.rotation.eulerAngles.y, 0);
        }

        if (coupleZ)
        {
            eulerTarget += new Vector3(0, 0, target.rotation.eulerAngles.z);
        }

        return eulerTarget;
    }

    // calculate polar coordinates behind target object
    void ThirdPersonPosition()
    {
        float rotation = Mathf.Deg2Rad * target.rotation.eulerAngles.y;

        float x = xOffset * Mathf.Sin(rotation);
        float z = xOffset * Mathf.Cos(rotation);

        Vector3 targetPosition = new Vector3(x, yOffset, z);

        transform.position = target.position + targetPosition;

        if (coupleThirdPersonRotation)
        {
            ThirdPersonRotation();
        }
    }

    void ThirdPersonRotation()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, target.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Zoom"))
        {
            fixedOffset = false;
        }
    }
}
