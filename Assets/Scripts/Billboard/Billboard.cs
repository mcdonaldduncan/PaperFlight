using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public float angleX = 90;
    public float angleY = 0;
    public float angleZ = 0;

    void Update()
    {
        Vector3 eulerRotation = Camera.main.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRotation.x + angleX, eulerRotation.y + angleY, eulerRotation.z + angleZ);
    }
}
