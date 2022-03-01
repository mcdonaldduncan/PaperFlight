using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    Transform parentTransform;
    float lastY;
    float rate;
    float z = 1;

    // Start is called before the first frame update
    void Start()
    {
        parentTransform = this.transform.parent.parent.parent;
        Debug.Log(parentTransform.name);
        lastY = parentTransform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(lastY != parentTransform.rotation.eulerAngles.y)
        {
            if(lastY > parentTransform.rotation.eulerAngles.y) //increasing
            {
                rate = Mathf.Abs(lastY - parentTransform.rotation.eulerAngles.y);

                if(rate > 1)
                {
                    rate = 1;
                }

                z -= rate;
                //Debug.Log("Decreasing " + parentTransform.rotation.eulerAngles.y);
                //Debug.Log("z " + z);
                //Debug.Log("Rate " + rate);
            }
            else //decreasing
            {
                rate = Mathf.Abs(lastY - parentTransform.rotation.eulerAngles.y);
                if (rate > 1)
                {
                    rate = 1;
                }
                z += rate;
                //Debug.Log("z " + z);
                //Debug.Log("Rate " + rate);
            }

            //if(z > 20)
       
            //     {
            //    z = 20;
            //}
            //else if (z < -20)
            //{
            //    z = -20;
            //}

            // the larger z is, the faster it changes
            lastY = parentTransform.rotation.eulerAngles.y;
        }
        else
        {
            Debug.Log("Blehh");
        }

        Quaternion localRotation = Quaternion.Euler(z, 0, 0);
        this.transform.localRotation = localRotation;
    }
}
