using System;
using System.Collections.Generic;
using UnityEngine;
public class SplineWalker : MonoBehaviour
{
    [Serializable] public struct TimePoints 
    {
        [Tooltip("Start Point")] public float PointA;
        [Tooltip("End Point")] public float PointB;
        [Tooltip("Amount of slow added. A higher number means slower speed in between those points.")] 
        public float durationFactor;
    }
    [Tooltip("Plotted points where speed can be adjusted between point A and point B.")] [SerializeField] 
    private List<TimePoints> timePoints;

    [SerializeField] private BezierSpline spline;

    [Tooltip("The total amount of duration of the plane's journey from the start to end of a spline.")] 
    [SerializeField] private float totalDuration;

    private float initialDuration;
    private float progress;

    private void Start()
    {
        initialDuration = totalDuration;
    }

    private void Update()
	{
        MoveAlongSpline();
        RotateAlongSpline();
    }

    void MoveAlongSpline()
    {
        if (progress < 1f)
        {
            if (timePoints.Count > 0)
            {
                for (int i = 0; i < timePoints.Count; i++)
                {
                    ChangeSpeed(i);// slow or speed up travel along spline to make things smoother
                }
            }

            progress += Time.deltaTime / totalDuration; // iterate current point along spline

            transform.position = spline.GetPoint(progress); // movement - set position to iterated point
        }
        else
        {
            progress = 1f; // finished
        }      
    }

    void RotateAlongSpline()
    { 
        transform.LookAt(transform.position + spline.GetDirection(progress)); 
    }

    void ChangeSpeed(int i)
    {
        if (progress >= timePoints[i].PointA && progress <= timePoints[i].PointB) // if we are in between A and B
        {
            totalDuration = Mathf.Lerp(initialDuration, totalDuration * timePoints[i].durationFactor, Time.deltaTime);
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.yellow);
        }
        else if (progress > timePoints[i].PointB) // outside of points, set to default speed
        {
            totalDuration = Mathf.Lerp(totalDuration, initialDuration, Time.deltaTime);
            timePoints.RemoveAt(i);
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.red);
        }
    }
}

//    // ---- Move along spline
//    [SerializeField] private List<BezierSpline> splines;
//    [SerializeField] private float duration;
//    [SerializeField] private float speed;
//    [SerializeField] private bool lookForward;
//    private BezierSpline currentSpline;
//    private int currentSplineIndex = 0;
//    private float splineProgress;

//    // ---- Move to next node
//    [SerializeField] private float nextNodeRotationSpeed;
//    bool hasSetNewRotation;
//    bool isNextNodeValid;
//    bool isOnSpline;
//    private int nodeIndex = 0;
//    private Vector3 nextNode;
//    private List<Vector3> nodes;
//    private Vector3 targetAngle;
//    private Vector3 currentAngle;

//    float step;

//    Vector3 startPositionBeforeMoveToNextNode;

//    private float nodeProgress;

//    public void Start()
//    {
//        //init 
//        nodes = new List<Vector3>(splines.Count);

//        if (splines.Count > 0)
//        {
//            for (int i = 0; i < splines.Count; i++)
//            {
//                nodes.Add(splines[i].GetPoint(0));
//            }
//        }

//        // current spline and node are first spline
//        currentSpline = splines[0];
//        nextNode = nodes[0];

//        // set initial angles for lerp between nodes
//        targetAngle = nextNode;
//        currentAngle = transform.eulerAngles;

//        //startTime = Time.time;
//        startPositionBeforeMoveToNextNode = transform.position;
//    }

//    private void Update()
//    {
//        // if current spline is finished
//        if (splineProgress >= 1f)
//        {
//            isOnSpline = false;
//            splineProgress = 0f;
//            nodeIndex++;
//            currentSplineIndex++;
//            nextNode = nodes[nodeIndex];
//            currentSpline = splines[currentSplineIndex];
//            targetAngle = currentSpline.GetDirection(0f);
//            isNextNodeValid = false;
//            hasSetNewRotation = false;
//        }

//        if (isOnSpline)
//        {
//            MoveOnSpline();
//            // iterate p distance over time
//            splineProgress += Time.deltaTime / duration;
//        }
//        else
//        {
//            LeaveSpline();
//            nodeProgress += Time.deltaTime / duration;
//        }

//        Debug.Log("Spline: " + splineProgress);

//        Debug.Log("Node: " + nodeProgress);
//    }

//	void MoveOnSpline()
//    {
//        Vector3 position = currentSpline.GetPoint(Time.deltaTime / duration);
//        transform.localPosition = position;

//        if (lookForward)
//		{
//			transform.LookAt(position + currentSpline.GetDirection(splineProgress));
//		}
//	}

//    private void LeaveSpline()
//    {
//        // Checks if the next node is valid once
//        if (!isNextNodeValid)
//        {
//            if (nodes[nodeIndex] != null)
//                isNextNodeValid = true;
//            else
//                Debug.LogError("Node at " + nodes[nodeIndex] + " Is invalid!");
//        }

//        MoveToNextNode();

//        if (Vector3.Distance(transform.position, nodes[nodeIndex]) < .001f)
//        {
//            isOnSpline = true;
//            nodeProgress = 0;
//            Debug.Log("FoundSpline");
//        }
//    }

//    void MoveToNextNode()
//    {
//        transform.position = Vector3.Lerp(transform.position, nextNode, nodeProgress);
//        SetPlaneRotation();
//    }

//    void SetPlaneRotation()
//    {
//        if (!hasSetNewRotation)
//        {
//            currentAngle = transform.position;
//            hasSetNewRotation = true;
//        }

//        Vector3 lookDirection = currentAngle + targetAngle;
//        lookDirection.Normalize();

//        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), nextNodeRotationSpeed * Time.deltaTime);
//        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.yellow);
//    }
//}