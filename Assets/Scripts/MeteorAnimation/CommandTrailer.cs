using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTrailer : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] int rotationStart;
    [SerializeField] int bracket;

    float rotation;
    float offset;
    
    Vector3 startPos;

    void Start()
    {
        offset = 5f;
        offset -= bracket;
        startPos = transform.position;
        rotation = Mathf.Deg2Rad * (45 * rotationStart);
    }

    void Update()
    {
        Rotate();
    }

    void Rotate()
    {

        rotation += Time.deltaTime;

        float x = offset * Mathf.Sin(rotation);
        float y = offset * Mathf.Cos(rotation);

        Vector3 targetPosition = new Vector3(x, y, -bracket * 2);

        transform.localPosition = target.localPosition + targetPosition;


        float rotationAmount = Mathf.Deg2Rad * Vector3.Angle(startPos, transform.position);

        Vector3 toRotate = new Vector3(0, 0, rotationAmount);

        transform.rotation = Quaternion.Euler(toRotate);
    }
}
