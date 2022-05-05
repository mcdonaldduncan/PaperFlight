using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using System;
using System.Collections.Generic;
using UnityEngine;
public class SplineWalker : MonoBehaviour
{
    [Serializable]
    private struct TimePoints
    {
        [Tooltip("Start Point")] public float PointA;
        [Tooltip("End Point")] public float PointB;
        [Tooltip("Amount of slow added. A higher number means slower speed in between those points.")]
        public float durationFactor;
    }

    [Tooltip("Plotted points where speed can be adjusted between point A and point B.")]
    [SerializeField] private List<TimePoints> timePointsList;

    [Tooltip("The total amount of duration of the plane's journey from the start to end of a spline.")]
    [SerializeField] private float totalDuration;

    [SerializeField] private BezierSpline spline;

    private float lerpStartTime;

    private float initialDuration;
    private float progress;
    private TimePoints currentTimePoints;

    private float targetDuration;

    private int pointIndex = 0;

    bool hasReachedPointA = false;
    bool hasReachedHalfway = false;
    bool hasReachedPointB = false;

    float halfway;

    [SerializeField] private bool usingControllers;
    private IVRInputDevice inputDevice;
    private string vrAxisTwo;

    private void Start()
    {
        initialDuration = totalDuration;
        progress = .35f;
        currentTimePoints = timePointsList[0];
    }

    private void Update()
    {
        MoveAlongSpline();
        RotateAlongSpline();

        if (usingControllers)
            SetTriggerInputValue();

        Debug.Log(totalDuration);
        Debug.Log(progress);
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
        transform.LookAt(transform.position + spline.GetDirection(progress));
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

            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.blue);
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

        if (pointIndex < timePointsList.Count)
        {
            pointIndex++;
            currentTimePoints = timePointsList[pointIndex];
        }

    }

    private void SetTriggerInputValue()
    {
        float maxDuration = initialDuration;

        if (inputDevice == null)
        {
            inputDevice = VRDevice.Device.PrimaryInputDevice;
            return;
        }

        float step = Time.deltaTime * .005f;

        float speedGoal = maxDuration / (1 + inputDevice.GetAxis1D(vrAxisTwo));

        totalDuration = Mathf.MoveTowards(totalDuration, speedGoal, step);
    }
}


//using System;
//using System.Collections.Generic;
//using UnityEngine;
//public class SplineWalker : MonoBehaviour
//{

//}




//[Serializable]
//private struct TimePoints
//{
//    [Tooltip("Start Point percentage e.g. .1")] public float PointA;
//    [Tooltip("End Point percentage e.g. .2")] public float PointB;
//    [Tooltip("The speed which we will descend to halfway between pointA and pointB.")]
//    public float targetSpeed;
//}

//[Tooltip("Plotted points where speed can be adjusted between point A and point B.")]
//[SerializeField] private List<TimePoints> timePointsList;

//[Tooltip("The total amount of duration of the plane's journey from the start to end of a spline.")]
//[SerializeField] private float speed;

//[SerializeField] private BezierSpline spline;

//private float lerpStartTime;
//private float initialSpeed;
//private float progress;

//private TimePoints currentTimePoints;

//private float targetSpeed;

//private int pointIndex = 0;

//private bool hasReachedPointA = false;
//private bool hasReachedHalfway = false;
//private bool hasReachedPointB = false;

//private float halfway;

//[SerializeField] private bool usingControllers;
//private IVRInputDevice inputDevice;
//private string vrAxisTwo;

//private void Start()
//{
//    speed /= 1000;
//    progress = .4f;
//    vrAxisTwo = VRAxis.Two;
//    initialSpeed = speed;
//}

//private void Update()
//{
//    MoveAlongSpline();
//    RotateAlongSpline();

//    //Debug.Log(progress);
//    Debug.Log(speed);

//    //if (usingControllers)
//    //SetTriggerInputValue();
//}

//void MoveAlongSpline()
//{
//    if (progress < 1f)
//    {
//        if (timePointsList.Count > 0)
//        {
//            ChangeSpeed();// slow or speed up travel along spline to make things smoother
//        }

//        progress += Time.deltaTime * speed; // iterate current point along spline

//        transform.position = spline.GetPoint(progress); // movement - set position to iterated point
//    }
//    else
//    {
//        progress = 1f; // finished
//    }
//}

//void RotateAlongSpline()
//{
//    transform.LookAt(transform.position + spline.GetDirection(progress));
//}

//void ChangeSpeed()
//{
//    if (progress >= currentTimePoints.PointA && progress <= currentTimePoints.PointB) // if we are in between A and B, slow down. 1st half of curve
//    {
//        if (!hasReachedPointA)
//        {
//            SetLerpValuesAtPointA();
//            hasReachedPointA = true;
//        }

//        if (progress < halfway) // before halfway to point B
//        {
//            LerpToHalfway();
//        }
//        else //after half to point B
//        {
//            if (!hasReachedHalfway)
//            {
//                SetLerpValuesAtHalfway();
//                hasReachedHalfway = true;
//            }

//            LerpToPointB();

//            if (initialSpeed - speed < .001f) // finished
//            {
//                if (!hasReachedPointB)
//                {
//                    SetLerpValuesAtPointB();
//                    hasReachedPointB = true;
//                }
//            }
//        }
//    }
//    else // outside of any time points. Reset flags
//    {
//        if (hasReachedPointA)
//            hasReachedPointA = false;

//        if (hasReachedHalfway)
//            hasReachedHalfway = false;

//        if (hasReachedPointB)
//            hasReachedPointB = false;
//    }
//}

//private void SetLerpValuesAtPointA()
//{
//    lerpStartTime = Time.time;

//    initialSpeed = speed;

//    targetSpeed = currentTimePoints.targetSpeed / 1000;

//    halfway = (currentTimePoints.PointA + currentTimePoints.PointB) / 2;
//}

//private void LerpToHalfway()
//{
//    float timeSinceStarted = Time.time - lerpStartTime;
//    float percentageComplete = timeSinceStarted;

//    speed = Mathf.Lerp(initialSpeed, targetSpeed, percentageComplete);
//}

//private void SetLerpValuesAtHalfway()
//{
//    lerpStartTime = Time.time;
//    targetSpeed = speed;
//}
//private void LerpToPointB()
//{
//    float timeSinceStarted = Time.time - lerpStartTime;
//    float percentageComplete = timeSinceStarted;

//    speed = Mathf.Lerp(targetSpeed, initialSpeed, percentageComplete);
//}

//private void SetLerpValuesAtPointB()
//{
//    speed = initialSpeed;
//    if (pointIndex != timePointsList.Count - 1)
//    {
//        pointIndex++;
//        currentTimePoints = timePointsList[pointIndex];
//    }
//}
//private void SetTriggerInputValue()
//{
//    float maxSpeed = initialSpeed;

//    if (inputDevice == null)
//    {
//        inputDevice = VRDevice.Device.PrimaryInputDevice;
//        return;
//    }

//    float step = Time.deltaTime * .005f;

//    float speedGoal = maxSpeed / (1 + inputDevice.GetAxis1D(vrAxisTwo));

//    speed = Mathf.MoveTowards(speed, speedGoal, step);
//}