using System;
using System.Collections.Generic;
using UnityEngine;
public class SplineWalker : MonoBehaviour
{
    [Serializable]
    public struct TimePoints
    {
        [Tooltip("Start Point")] public float PointA;
        [Tooltip("End Point")] public float PointB;
        [Tooltip("Amount of slow added. A higher number means slower speed in between those points.")]
        public float durationFactor;
    }
    [Tooltip("Plotted points where speed can be adjusted between point A and point B.")]
    [SerializeField]
    private List<TimePoints> timePointsList;

    [SerializeField] private BezierSpline spline;

    [Tooltip("The total amount of duration of the plane's journey from the start to end of a spline.")]
    [SerializeField] private float totalDuration;

    private float timeTakenDuringLerp = 4f;

    private float lerpStartTime;

    private float initialDuration;
    private float progress;
    private TimePoints currentTimePoints;
    private TimePoints nextTimePoints;

    private float targetDuration;

    private float lastYRotation = 0;
    private float rollSpeed;
    private Transform rotN;

    private bool valuesSet;

    private int pointIndex = 0;
    bool targetsSet;
    bool distanceSet = false;

    float rollAngle;
    float rateOfChange = .0008f;

    bool hasReachedPointA = false;
    bool hasReachedHalfway = false;
    bool hasReachedPointB = false;

    float halfway;

    float lerpSpeed;

    private void Start()
    {
        initialDuration = totalDuration;

        if(timePointsList.Count > 0)
            currentTimePoints = timePointsList[0];

        lastYRotation = transform.rotation.eulerAngles.y;
        rotN = transform.GetChild(1).GetChild(0).GetChild(0);
    }

    private void Update()
    {
        MoveAlongSpline();
        RotateAlongSpline();

        //Debug.Log(totalDuration);

        //Debug.Log(pointIndex);
    }

    void MoveAlongSpline()
    {
        if (progress < 1f)
        {
            if (timePointsList.Count > 0)
            {
                ChangeSpeed();// slow or speed up travel along spline to make things smoother
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
        transform.LookAt((transform.position + spline.GetDirection(progress)), spline.GetDirection(progress)); // rotates pitch and yaw

        //RotateRoll();

        Debug.DrawRay(spline.GetPoint(progress), new Vector3(0, spline.GetPoint(progress).y * 10, 0), Color.yellow);
    }

    void ChangeSpeed()
    {
        if (progress >= currentTimePoints.PointA && progress <= currentTimePoints.PointB) // if we are in between A and B, slow down. 1st half of curve
        {
            if (!hasReachedPointA)
            {
                SetLerpValuesAtPointA();
                hasReachedPointA = true;
            }

            if (progress < halfway) // before halfway to point B
            {
                LerpToHalfway();
            }
            else //after half to point B
            {
                if (!hasReachedHalfway)
                {
                    SetLerpValuesAtHalfway();
                    hasReachedHalfway = true;
                }

                LerpToPointB();

                if (totalDuration - initialDuration < .001f) // finished
                {
                    if (!hasReachedPointB)
                    {
                        SetLerpValuesAtPointB();
                        hasReachedPointB = true;
                    }
                }
            }
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.yellow);
        }
        else // before any time points have been reached or after they are all done
        {
            if (hasReachedPointA)
                hasReachedPointA = false;

            if (hasReachedHalfway)
                hasReachedHalfway = false;

            if (hasReachedPointB)
                hasReachedPointB = false;

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.blue);
        }
    }

    private void SetLerpValuesAtPointA()
    {
        lerpStartTime = Time.time;

        initialDuration = totalDuration;

        targetDuration = totalDuration + currentTimePoints.durationFactor;

        halfway = (currentTimePoints.PointA + currentTimePoints.PointB) / 2;
    }

    private void LerpToHalfway()
    {
        float timeSinceStarted = Time.time - lerpStartTime;
        float percentageComplete = timeSinceStarted;

        totalDuration = Mathf.Lerp(initialDuration, targetDuration, percentageComplete);
    }

    private void SetLerpValuesAtHalfway()
    {
        lerpStartTime = Time.time;
        targetDuration = totalDuration;
    }
    private void LerpToPointB()
    {
        float timeSinceStarted = Time.time - lerpStartTime;
        float percentageComplete = timeSinceStarted;

        totalDuration = Mathf.Lerp(targetDuration, initialDuration, percentageComplete);
    }

    private void SetLerpValuesAtPointB()
    {
        totalDuration = initialDuration;
        pointIndex++;

        //if (pointIndex < timePointsList.Count - 1) // set next
        //{
        //    nextTimePoints = timePointsList[pointIndex + 1];
        //}
        //else
        //{
        //    nextTimePoints = currentTimePoints;
        //}

        currentTimePoints = timePointsList[pointIndex];
    }

    void RotateRoll()
    {
        rollSpeed = Mathf.Abs(lastYRotation - this.transform.rotation.eulerAngles.y);

        //float test = 5;

        //if (rollSpeed > 1.5)
        //    rollSpeed = 1.5f;

        //if (lastYRotation < this.transform.rotation.eulerAngles.y) // Angle larger than 5 and increasing
        //{
        //    rateOfChange = rollSpeed * .5f;
        //    rollSpeed -= rateOfChange;           
        //    Debug.Log("Rate " + rateOfChange);
        //}
        //else if (lastYRotation > this.transform.rotation.eulerAngles.y) // Angle less than 5 and decreasing
        //{
        //    rateOfChange = rollSpeed * .5f;
        //    rollSpeed -= rateOfChange;
        //    Debug.Log("Rate " + rateOfChange);
        //}

        //if (rollSpeed < 0)
        //    rollSpeed = 0;

        //if (rollAngle > 22)
        //    rollAngle = 22;
        //else if (rollAngle < -22)
        //    rollAngle = -22;
          
            //Debug.Log(rollSpeed);

        //if (lastYRotation > this.transform.rotation.eulerAngles.y)
        //    rollAngle -= rollSpeed;                                     //decreasing
        //else
        //    rollAngle += rollSpeed;                                     //increasing

        //lastYRotation = this.transform.rotation.eulerAngles.y;

        //rotN.gameObject.transform.localRotation = Quaternion.Euler(rollAngle, 0, 0);
    }
}
//    private void Start()
//    {
//        initialDuration = totalDuration;
//        currentTimePoints = timePointsList[0];
//    }
//    private void Update()
//    {
//        MoveAlongSpline();
//        RotateAlongSpline();

//        //Debug.Log(Mathf.Round(progress * 100f) / 100f);
//        //Debug.Log(nextTimePoints.PointA + " " + nextTimePoints.PointB);



//        if (Mathf.Round(progress * 100f) / 100f == halfway)
//        {
//            lerpStartTime = Time.time;
//            targetDuration = totalDuration;
//        }
//    }

//    void MoveAlongSpline()
//    {
//        if (progress < 1f)
//        {
//            if (timePointsList.Count > 0)
//            {
//                ChangeSpeed();// slow or speed up travel along spline to make things smoother
//            }

//            progress += Time.deltaTime / totalDuration; // iterate current point along spline

//            transform.position = spline.GetPoint(progress); // movement - set position to iterated point
//        }
//        else
//        {
//            progress = 1f; // finished
//        }
//    }

//    void RotateAlongSpline()
//    {
//        transform.LookAt(transform.position + spline.GetDirection(progress));
//    }

//    void ChangeSpeed()
//    {
//        if (timePointsList.Count > 1) // todo change to only once
//        {
//            currentTimePoints = timePointsList[pointIndex];
//            nextTimePoints = timePointsList[pointIndex + 1];
//        }
//        else
//        {
//            nextTimePoints = currentTimePoints;
//        }

//        if (progress >= currentTimePoints.PointA && progress <= currentTimePoints.PointB) // if we are in between A and B, slow down. 1st half of curve
//        {
//            if(!valuesSet)
//            {
//                lerpStartTime = Time.time;

//                initialDuration = totalDuration;

//                targetDuration = totalDuration + currentTimePoints.durationFactor;

//                halfway = (currentTimePoints.PointA + currentTimePoints.PointB) / 2;

//                halfwayPosition = new Vector3(spline.GetPoint(halfway).x, spline.GetPoint(halfway).y, spline.GetPoint(halfway).z);

//                Debug.Log("HALF " + halfwayPosition);

//                valuesSet = true;
//            }

//            if (progress < halfway) // before halfway to point B
//            {
//                float timeSinceStarted = Time.time - lerpStartTime;
//                float percentageComplete = timeSinceStarted;

//                totalDuration = Mathf.Lerp(initialDuration, targetDuration, percentageComplete);
//            }
//            else //after half to point B
//            {              
//                float timeSinceStarted = Time.time - lerpStartTime;
//                float percentageComplete = timeSinceStarted;

//                totalDuration = Mathf.Lerp(targetDuration, initialDuration, percentageComplete);

//                if (totalDuration - initialDuration < .0001f) // finished
//                {
//                    totalDuration = initialDuration;
//                    //timePointsList.RemoveAt(pointIndex);
//                    pointIndex++;
//                    currentTimePoints = timePointsList[pointIndex];
//                }
//            }          

//            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.yellow);
//        }
//        else if (progress > currentTimePoints.PointB && progress < nextTimePoints.PointA) // In between two points
//        {           
//            if(valuesSet)
//                valuesSet = false;
//            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.red);
//        }
//        else // before any time points have been reached or after they are all done
//        {
//            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.blue);
//        }
//    }
//}

//float t = Time.deltaTime * 1 / (progress - nextTimePoints.PointA);
//t = t * t * (3f - 2f * t);
//totalDuration = Mathf.Lerp(initialDuration, totalDuration * currentTimePoints.durationFactor, t);

//if (progress > currentTimePoints.PointA && progress < currentTimePoints.PointA + .01f) // super super hacky
//{
//    _timeStartedLerping = Time.time;

//    initialDuration = totalDuration;
//}

//float timeSinceStarted = Time.time - _timeStartedLerping;
//float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

////totalDuration = Mathf.Lerp(totalDuration, initialDuration, percentageComplete);
////float t = Time.deltaTime * 1 / (progress - nextTimePoints.PointA);
////t = t * t * (3f - 2f * t);
//totalDuration = Mathf.Lerp(totalDuration, initialDuration, percentageComplete);

//using System;
//using System.Collections.Generic;
//using UnityEngine;
//public class SplineWalker : MonoBehaviour
//{
//    [Serializable] public struct TimePoints 
//    {
//        [Tooltip("Start Point")] public float PointA;
//        [Tooltip("End Point")] public float PointB;
//        [Tooltip("Amount of slow added. A higher number means slower speed in between those points.")] 
//        public float durationFactor;
//    }
//    [Tooltip("Plotted points where speed can be adjusted between point A and point B.")] [SerializeField] 
//    private List<TimePoints> timePointsList;

//    [SerializeField] private BezierSpline spline;

//    [Tooltip("The total amount of duration of the plane's journey from the start to end of a spline.")] 
//    [SerializeField] private float totalDuration;

//    private float initialDuration;
//    private float progress;
//    private TimePoints currentTimePoints;
//    private TimePoints nextTimePoints;

//    private int pointIndex = 0;
//    bool targetsSet;
//    bool distanceSet = false;

//    float lerpSpeed;

//    private void Start()
//    {
//        initialDuration = totalDuration;

//        currentTimePoints = timePointsList[0];
//    }

//    private void Update()
//	{
//        MoveAlongSpline();
//        RotateAlongSpline();

//        Debug.Log(totalDuration);
//    }

//    void MoveAlongSpline()
//    {
//        if (progress < 1f)
//        {
//            if (timePointsList.Count > 0)
//            {
//                ChangeSpeed();// slow or speed up travel along spline to make things smoother
//            }

//            progress += Time.deltaTime / totalDuration; // iterate current point along spline

//            transform.position = spline.GetPoint(progress); // movement - set position to iterated point
//        }
//        else
//        {
//            progress = 1f; // finished
//        }      
//    }

//    void RotateAlongSpline()
//    { 
//        transform.LookAt(transform.position + spline.GetDirection(progress)); 
//    }

//    void ChangeSpeed()
//    {
//        if(!distanceSet) // run once block
//        {
//            lerpSpeed = (1 / GetDistanceToNextTimePoint())/20; // inverse function. Smaller the distance, greater the lerp speed

//            if(timePointsList.Count > 1)
//            {
//                currentTimePoints = timePointsList[pointIndex];
//                nextTimePoints = timePointsList[pointIndex + 1];
//            }
//            else
//            {
//                nextTimePoints = currentTimePoints;
//            }
//        }

//        if (progress >= currentTimePoints.PointA && progress <= currentTimePoints.PointB) // if we are in between A and B, slow down. 1st half of curve
//        {
//            float t = Time.deltaTime * 1 / (progress - nextTimePoints.PointA);
//            t = t * t * (3f - 2f * t);
//            totalDuration = Mathf.Lerp(initialDuration, totalDuration * currentTimePoints.durationFactor, t);
//            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.yellow);
//            GetDistanceToNextTimePoint();
//        }
//        else if (progress > currentTimePoints.PointB && progress < nextTimePoints.PointA && totalDuration != initialDuration) // In between two points, speed back up. 2nd half of curve
//        {
//            float t = Time.deltaTime * 1 / (progress - nextTimePoints.PointA);
//            t = t * t * (3f - 2f * t);
//            totalDuration = Mathf.Lerp(totalDuration, initialDuration, t);

//            if (totalDuration - initialDuration < .1f)
//            {
//                totalDuration = initialDuration;
//                timePointsList.RemoveAt(pointIndex);
//                currentTimePoints = timePointsList[pointIndex];
//            }

//            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.red);
//        }
//        else // before any time points have been reached or after they are all done
//        {
//            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.blue);
//        }   
//    }

//    float GetDistanceToNextTimePoint() // do once
//    {
//        if(timePointsList.Count > 1) // if there is a next time point
//        {
//            return timePointsList[pointIndex + 1].PointA - currentTimePoints.PointB;          
//        }
//        else
//        {
//            return .5f;
//        }

//    }
//}

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