using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
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

    bool isZoomingOut;
    bool zoomOffsetSet;
    Vector3 zoomOffset;

    
    void LateUpdate()
    {
        if (isZoomingOut)
        {
            walker.enabled = false;
            if (!zoomOffsetSet)
            {
                zoomOffset = new Vector3(transform.localPosition.x - 5, transform.localPosition.y + 7, transform.localPosition.z);
                zoomOffsetSet = true;
            }

            if (Vector3.Distance(transform.localPosition, zoomOffset) < .01f)
                isZoomingOut = false;

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, zoomOffset, .5f * Time.deltaTime);
        }
        else
        {
            OffsetTargetFollow();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zoom"))
        {
            isZoomingOut = true;
        }
    }
}
