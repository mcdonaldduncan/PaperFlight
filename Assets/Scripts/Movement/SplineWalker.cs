using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;
using System;
using System.Collections.Generic;
using UnityEngine;
public class SplineWalker : MonoBehaviour
{
    [SerializeField] float startProgress;
    [SerializeField] float additionalDuration;

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
    private TimePoints nextTimePoints;
    private bool usingTimePoints;

    private float targetDuration;

    private int pointIndex = 0;

    private bool hasReachedPointA = false;
    private bool hasReachedHalfway = false;
    private bool hasReachedPointB = false;

    private float halfway;

    [SerializeField] private float maxIncrease;
    private float currentIncrease;

    [SerializeField] private bool usingControllers;
    private IVRInputDevice inputDevice;
    private string vrAxisTwo;

    private void Start()
    {
        initialDuration = totalDuration;
        currentTimePoints = timePointsList[0];

        progress = startProgress;
        vrAxisTwo = VRAxis.Two;

        if (timePointsList.Count > 0)
        {
            usingTimePoints = true;

            if (timePointsList.Count > 1)
                nextTimePoints = timePointsList[1];
        }
    }

    private void Update()
    {
        MoveAlongSpline();
        RotateAlongSpline();

        if (usingControllers)
            additionalDuration = SetTriggerInputValue();


        //Debug.Log(progress);
    }

    void MoveAlongSpline()
    {
        if (progress < 1f)
        {
            if (usingTimePoints)
            {
                ChangeSpeed();// slow or speed up travel along spline to make things smoother
            }

            progress += Time.deltaTime / totalDuration + additionalDuration; // iterate current point along spline

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
            //if(progress >= nextTimePoints.PointA && progress <= nextTimePoints.PointB)
            //{
            //    currentTimePoints = nextTimePoints;

            //    if (pointIndex + 1 >= timePointsList.Count)
            //        pointIndex++;

            //    return;
            //}
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
        }
        else // before any time points have been reached or after they are all done
        {
            if (hasReachedPointA)
                hasReachedPointA = false;

            if (hasReachedHalfway)
                hasReachedHalfway = false;

            if (hasReachedPointB)
                hasReachedPointB = false;
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

        totalDuration = Mathf.Lerp(targetDuration, initialDuration, timeSinceStarted);
    }


    private void SetLerpValuesAtPointB()
    {
        totalDuration = initialDuration;
        pointIndex++;

        if (pointIndex < timePointsList.Count)
        {
            currentTimePoints = timePointsList[pointIndex];
        }
    }

    private float SetTriggerInputValue()
    {
        //float maxDuration = initialDuration;

        if (inputDevice == null)
        {
            inputDevice = VRDevice.Device.PrimaryInputDevice;
        }


        if (inputDevice.GetAxis1D(VRAxis.Two) > 0)
        {
            currentIncrease = maxIncrease;
        }
        else
        {
            currentIncrease = 0f;
        }
        //if (inputDevice.GetAxis1D(VRAxis.Two) == 0)
        //{
        //    currentIncrease = 0;
        //}

        float targetDuration = currentIncrease;

        //float step = Time.deltaTime;
        //float speedGoal = maxDuration / (1 + inputDevice.GetAxis1D(vrAxisTwo));
        //totalDuration = Mathf.MoveTowards(totalDuration, targetDuration, step);

        return targetDuration;
    }
}