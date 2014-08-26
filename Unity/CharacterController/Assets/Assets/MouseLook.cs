#if UNITY_ANDROID && !UNITY_EDITOR
using tv.ouya.console.api;
#endif

using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;

    private const float DEADZONE = 0.25f;

    float ApplyDeadZone(float val)
    {
        if (Mathf.Abs(val) < DEADZONE)
        {
            val = 0f;
        }
        return val;
    }

	void Update ()
	{
		if (axes == RotationAxes.MouseXAndY)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
            float rotationX = transform.localEulerAngles.y + ApplyDeadZone(OuyaSDK.OuyaInput.GetAxis(0, OuyaController.AXIS_LS_X)) * sensitivityX;
#else
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
#endif
		
#if UNITY_ANDROID && !UNITY_EDITOR
            rotationY += -ApplyDeadZone(OuyaSDK.OuyaInput.GetAxis(0, OuyaController.AXIS_LS_Y)) * sensitivityY;
#else
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
#endif
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
            transform.Rotate(0, ApplyDeadZone(OuyaSDK.OuyaInput.GetAxis(0, OuyaController.AXIS_LS_X)) * sensitivityX, 0);
#else
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
#endif
		}
		else
		{
#if UNITY_ANDROID && !UNITY_EDITOR
            rotationY += -ApplyDeadZone(OuyaSDK.OuyaInput.GetAxis(0, OuyaController.AXIS_LS_Y)) * sensitivityY;
#else
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
#endif
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}