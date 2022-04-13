using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public float angleX = 90;
    public float angleY = 0;
    public float angleZ = 0;

    Vector3 transformVec = new Vector3(90, 90, 90);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 eulerRotation = Camera.main.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRotation.x + angleX, eulerRotation.y + angleY, eulerRotation.z + angleZ);
    }
}
