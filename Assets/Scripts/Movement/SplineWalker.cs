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
        [Tooltip("Start Point percentage e.g. .1")] public float PointA;
        [Tooltip("End Point percentage e.g. .2")] public float PointB;
        [Tooltip("The speed which we will descend to halfway between pointA and pointB.")]
        public float targetDuration;
    }

    [Tooltip("Plotted points where speed can be adjusted between point A and point B.")]
    [SerializeField] private List<TimePoints> timePointsList;

    [Tooltip("The total amount of duration of the plane's journey from the start to end of a spline.")]
    [SerializeField] private float duration;

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

    public bool usingControllers;
    private IVRInputDevice inputDevice;
    private float triggerInputValue;


    private void Start()
    {
        initialDuration = duration;

        currentTimePoints = timePointsList[0];

        OVRManager.instance.useRecommendedMSAALevel = false;
    }

    private void Update()
    {
        MoveAlongSpline();
        RotateAlongSpline();

        //Debug.Log(speed);
        ////Debug.Log(progress);
        //if (inputDevice == null)
        //{
        //    inputDevice = VRDevice.Device.PrimaryInputDevice;
        //}

        //Debug.Log(VRDevice.DeviceName);

        //Debug.Log(inputDevice.GetAxis1D(VRAxis.Two));

        //if (usingControllers)
        //    SetTriggerInputValue();
    }

    void MoveAlongSpline()
    {
        if (progress < 1f)
        {
            if (timePointsList.Count > 0)
            {
                ChangeSpeed();// slow or speed up travel along spline to make things smoother
            }

            progress += Time.deltaTime / duration; // iterate current point along spline

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

                if (initialDuration - duration < .001f) // finished
                {
                    if (!hasReachedPointB)
                    {
                        SetLerpValuesAtPointB();
                        hasReachedPointB = true;
                    }
                }
            }
        }
        else // outside of any time points. Reset flags
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

        initialDuration = duration;

        halfway = (currentTimePoints.PointA + currentTimePoints.PointB) / 2;
    }

    private void LerpToHalfway()
    {
        float timeSinceStarted = Time.time - lerpStartTime;
        float percentageComplete = timeSinceStarted;

        duration = Mathf.Lerp(initialDuration, targetDuration, percentageComplete);
    }

    private void SetLerpValuesAtHalfway()
    {
        lerpStartTime = Time.time;
        targetDuration = duration;
    }
    private void LerpToPointB()
    {
        float timeSinceStarted = Time.time - lerpStartTime;
        float percentageComplete = timeSinceStarted;

        duration = Mathf.Lerp(targetDuration, initialDuration, percentageComplete);
    }

    private void SetLerpValuesAtPointB()
    {
        duration = initialDuration;
        if(pointIndex != timePointsList.Count -1)
        {
            pointIndex++;
            currentTimePoints = timePointsList[pointIndex];
        }
    }
    private void SetTriggerInputValue()
    {
        if (inputDevice == null)
        {
            inputDevice = VRDevice.Device.PrimaryInputDevice;
            return;
        }

        //Debug.Log(VRDevice.DeviceName);

        //Debug.Log(inputDevice.Name);

        //speed /= (1 + inputDevice.GetAxis1D(VRAxis.Two));
    }
}