using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePathing : MonoBehaviour
{
    [SerializeField] private List<Transform> nodes = new List<Transform>();
    [SerializeField] private GameObject paperPlane;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private BezierSpline spline;

    public float duration;

    private float progress;

    int nodeIndex;
    Transform nextNode;
    bool isNextNodeValid;

    bool hasSetNewRotation;

    Vector3 targetAngle;
    Vector3 currentAngle;

    void Start()
    {
        nodeIndex = 0;
        nextNode = nodes[nodeIndex];

        targetAngle = nextNode.position;
        currentAngle = transform.eulerAngles;
    }

    void Update()
    {
        if (progress > 1f)
        {
            progress = 1f;
        }
        Debug.Log(spline.GetPoint(progress));
        // We have reached the end
        if (nodeIndex < nodes.Count)
        {
            UpdatePlaneLocation();
        }
    }

    private void UpdatePlaneLocation()
    {
        // Checks if the next node is valid once
        if (!isNextNodeValid)
        {
            if(nodes[nodeIndex] != null)
                isNextNodeValid = true;
            else
                Debug.LogError("Node at " + nodes[nodeIndex].position + " Is invalid!");
        }

        MovePlaneToNextNode();

        if (Vector3.Distance(paperPlane.transform.position, nodes[nodeIndex].transform.position) < .001f)
        {
            nodeIndex++;

            if(nodeIndex < nodes.Count)
                nextNode = nodes[nodeIndex];

            targetAngle = nextNode.position;
            isNextNodeValid = false;
            hasSetNewRotation = false;
        }
    }

    private void MovePlaneToNextNode()
    {
        float step = moveSpeed * Time.deltaTime;
        paperPlane.transform.position = Vector3.MoveTowards(paperPlane.transform.position, nodes[nodeIndex].transform.position, step);
        
        if(!hasSetNewRotation)
        {
            Vector3 a = new Vector3(100, 0, 0);
            currentAngle = paperPlane.transform.position;
            hasSetNewRotation = true;
        }
       
        Vector3 lookDirection = targetAngle - currentAngle;
        lookDirection.Normalize();

        paperPlane.transform.rotation = Quaternion.Slerp(paperPlane.transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);
        Debug.DrawRay(paperPlane.transform.position, paperPlane.transform.TransformDirection(Vector3.forward), Color.yellow);

    }
}
