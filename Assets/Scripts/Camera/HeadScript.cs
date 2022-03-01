using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadScript : MonoBehaviour
{
    Transform planeTransform;
    // Start is called before the first frame update
    void Start()
    {
        //Vector3 offset = this.transform.localPosition;
        //rotationBone = transform.parent.parent.GetChild(1).GetChild(0).GetChild(0).gameObject;
        planeTransform = transform.parent;
        //this.transform.position = rotationBone.transform.position + offset;
        //this.transform.rotation = rotationBone.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        LockAxis();
    }

    void LockAxis()
    {
        //Vector3 rotation = this.transform.rotation.eulerAngles;
        //rotation.x = 0;

        //transform.rotation = Quaternion.Euler(rotation);
    }
}
