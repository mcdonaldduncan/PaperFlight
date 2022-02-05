using System.Collections;
using System.Collections.Generic;
using Liminal.Core.Fader;
using Liminal.Platform.Experimental.App.Experiences;
using Liminal.SDK.Core;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Avatars;
using Liminal.SDK.VR.Input;
using UnityEngine;

/* Testing movement and rotation options
 * Only axis inputs allowed are joysticks
 * Using non emulated environment tomorrow so I can map input to rotation and directional changes 
 */

public class MovementTest : MonoBehaviour
{
    [SerializeField] Transform paperPlane;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float moveValue = 1.0f;
    [SerializeField] float rotationSpeed = 1.0f;

    bool moveRight = false;
    bool moveLeft = false;
    bool shouldRotate = true;

    float minRotation = -45;
    float maxRotation = 45;

    Vector3 neutralRotation;

    private void Start()
    {
        neutralRotation = paperPlane.rotation.eulerAngles;
    }

    private void Update()
    {
        var avatar = VRAvatar.Active;
        if (avatar == null)
            return;

        var rightInput = GetInput(VRInputDeviceHand.Right);

        if (rightInput != null)
        {
            if (rightInput.GetButtonDown(VRButton.Back))
            {
                Debug.Log("Back button pressed");
                moveRight = !moveRight;
                moveLeft = false;
            }

            if (rightInput.GetButtonDown(VRButton.One))
            {
                Debug.Log("Trigger button pressed");
                moveLeft = !moveLeft;
                moveRight = false;
            }
        }
        
        //Any input
        //VRDevice.Device.GetButtonDown(VRButton.One);
    }

    //Use Lerp
    private void FixedUpdate()
    {
        if (moveRight)
        {
            paperPlane.position += Vector3.right * moveValue * Time.deltaTime;
            if (shouldRotate)
                paperPlane.Rotate(0, -rotationSpeed * Time.deltaTime, 0, Space.Self);
        }
        if (moveLeft)
        {
            paperPlane.position += Vector3.left * moveValue * Time.deltaTime;
            if (shouldRotate)
                paperPlane.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
        }
        if (!moveLeft && !moveRight)
        {
            //StartCoroutine(SmoothRotationReset());
        }

        paperPlane.position += Vector3.forward * speed * Time.deltaTime;

        if (paperPlane.rotation.y >= 45 || paperPlane.rotation.y <= -45)
            shouldRotate = false;
        else
            shouldRotate = true;
    }

    private IEnumerator SmoothRotationReset()
    {
        Vector3 currentRotation = paperPlane.rotation.eulerAngles;
        Vector3 deltaRotation = neutralRotation - currentRotation;

        if (currentRotation == neutralRotation)
        {
            yield break;
        }
        
        float timeDelta = 0.0f;
        while(timeDelta < 2.0f)
        {
            timeDelta += Time.deltaTime;
            paperPlane.Rotate(deltaRotation * Time.deltaTime, Space.Self);
            yield return null;
        }
    }

    private IVRInputDevice GetInput(VRInputDeviceHand hand)
    {
        var device = VRDevice.Device;
        return hand == VRInputDeviceHand.Left ? device.SecondaryInputDevice : device.PrimaryInputDevice;
    }

    /// <summary>
    /// End will only close the application when you're within the platform
    /// </summary>
    public void End()
    {
        ExperienceApp.End();
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeToBlackRoutine());
    }

    private IEnumerator FadeToBlackRoutine()
    {
        ScreenFader.Instance.FadeTo(Color.black, duration: 1);
        yield return ScreenFader.Instance.WaitUntilFadeComplete();
        ScreenFader.Instance.FadeToClear(duration: 1);
    }

}
