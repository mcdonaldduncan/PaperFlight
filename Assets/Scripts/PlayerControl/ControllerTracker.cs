using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTracker : MonoBehaviour
{
    [SerializeField] Transform nTransform;
    [SerializeField] Transform primaryHand;
    [SerializeField] Transform secondaryHand;
    [SerializeField] float speed;
    [SerializeField] float maxX;
    [SerializeField] float maxY;
    [SerializeField] float maxZ;
    [SerializeField] float rotationScale;
    [SerializeField] float rotationSpeed;

    float rotateX;
    float rotateY;
    float rotateZ;

    void Start()
    {

    }

    void Update()
    {
        SupplementalRotation();        
    }

    void SupplementalRotation()
    {
        rotateX = primaryHand.rotation.x * rotationScale;

        Vector3 clampedRotation = new Vector3(nTransform.rotation.x, nTransform.rotation.y, nTransform.rotation.z);

        clampedRotation.x = Mathf.Clamp(clampedRotation.x, -maxX, maxX);

        Quaternion clampedQuaternion = Quaternion.Euler(clampedRotation);

        nTransform.rotation = Quaternion.Slerp(nTransform.rotation, clampedQuaternion, rotationSpeed);

    }

    void Movement()
    {
        Debug.Log(primaryHand.rotation);

        Vector3 moveDirection = transform.TransformDirection(Vector3.forward);

        transform.position += moveDirection * speed * Time.deltaTime;

        Rotation();
    }

    void Rotation()
    {
        //Approach 1

        //rotateX = primaryHand.rotation.x * rotationScale;
        //rotateY = primaryHand.rotation.y * rotationScale;
        //rotateZ = primaryHand.rotation.z * rotationScale;

        //Vector3 clampedRotation = new Vector3(rotateX, rotateY, rotateZ);

        //clampedRotation.x = Mathf.Clamp(clampedRotation.x, -maxX, maxX);
        //clampedRotation.y = Mathf.Clamp(clampedRotation.y, -maxY, maxY);
        //clampedRotation.z = Mathf.Clamp(clampedRotation.z, -maxZ, maxZ);

        //Quaternion clampedQuaternion = Quaternion.Euler(clampedRotation);

        //transform.rotation = clampedQuaternion;

        //transform.Rotate(Vector3.forward, rotateZ);




        //Approach 2

        //rotateX = primaryHand.rotation.x;
        //rotateY = primaryHand.rotation.y;
        //rotateZ = primaryHand.rotation.z;

        //transform.Rotate(rotateX, rotateY, rotateZ);

        //float clampedX = Mathf.Clamp(transform.rotation.x, -maxX, maxX);
        //float clampedY = Mathf.Clamp(transform.rotation.y, -maxY, maxY);
        //float clampedZ = Mathf.Clamp(transform.rotation.z, -maxZ, maxZ);

        //transform.rotation = Quaternion.Euler(clampedX, clampedY, clampedZ);




        //Approach 3

        rotateX = primaryHand.rotation.x * rotationScale;
        rotateY = primaryHand.rotation.y * rotationScale;
        rotateZ = primaryHand.rotation.z * rotationScale;

        Vector3 clampedRotation = new Vector3(rotateX, rotateY, rotateZ);

        clampedRotation.x = Mathf.Clamp(clampedRotation.x, -maxX, maxX);
        clampedRotation.y = Mathf.Clamp(clampedRotation.y, -maxY, maxY);
        clampedRotation.z = Mathf.Clamp(clampedRotation.z, -maxZ, maxZ);

        Quaternion clampedQuaternion = Quaternion.Euler(clampedRotation);

        transform.rotation = Quaternion.Slerp(transform.rotation, clampedQuaternion, rotationSpeed);

    }
}
