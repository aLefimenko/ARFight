﻿// ***********************************************************
// Written by Heyworks Unity Studio http://unity.heyworks.com/
// ***********************************************************
using UnityEngine;

/// <summary>
/// Gyroscope controller that works with any device orientation.
/// </summary>
public class RotatePr : MonoBehaviour
{
    #region [Private fields]

    private bool gyroEnabled = true;
    private const float lowPassFilterFactor = 0.2f;

    private readonly Quaternion baseIdentity = Quaternion.Euler(0, 0, 0);
    private readonly Quaternion landscapeRight = Quaternion.Euler(0, 0, 90);
    private readonly Quaternion landscapeLeft = Quaternion.Euler(0, 0, -90);
    private readonly Quaternion upsideDown = Quaternion.Euler(0, 0, 180);

    private Quaternion cameraBase = Quaternion.identity;
    private Quaternion calibration = Quaternion.identity;
  //  private Quaternion calibration = Quaternion.Euler(0, 0, 0);
    private Quaternion baseOrientation = Quaternion.Euler(0, 0, 0);
    private Quaternion baseOrientationRotationFix = Quaternion.identity;

    private Quaternion referanceRotation = Quaternion.identity;
   // private Quaternion referanceRotation = Quaternion.Euler(0, 0, 0);
    private bool debug = true;

    #endregion

    #region [Unity events]

    protected void Start()
    {
        //BaseSDK.ResetQuat();
        AttachGyro();
        //Input.gyro.enabled = true;

    }

    protected void Update()
    {
        if (!gyroEnabled)
            return;
             // transform.rotation = Quaternion.Slerp(transform.rotation,
            //  cameraBase * (ConvertRotation(referanceRotation * BaseSDK.GetQuaternion()) * GetRotFix()), lowPassFilterFactor);
            transform.rotation = Quaternion.Slerp(transform.rotation, referanceRotation* BaseSDK.GetQuaternion(), lowPassFilterFactor);
       // GameObject.Find("Text1").GetComponent<UnityEngine.UI.Text>().text = (referanceRotation * BaseSDK.GetQuaternion()).x + " " + (referanceRotation * BaseSDK.GetQuaternion()).y;
       // GameObject.Find("Text2").GetComponent<UnityEngine.UI.Text>().text = transform.rotation.x + " " + transform.rotation.y + " " + transform.rotation.z;
        //GameObject.Find("Text1").GetComponent<UnityEngine.UI.Text>().text = referanceRotation.ToString();
    }

    //protected void OnGUI()
    //{
    //	if (!debug)
    //		return;

    //	GUILayout.Label("Orientation: " + Screen.orientation);
    //	GUILayout.Label("Calibration: " + calibration);
    //	GUILayout.Label("Camera base: " + cameraBase);
    //	GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);
    //	GUILayout.Label("transform.rotation: " + transform.rotation);

    //	if (GUILayout.Button("On/off gyro: " + Input.gyro.enabled, GUILayout.Height(100)))
    //	{
    //		Input.gyro.enabled = !Input.gyro.enabled;
    //	}

    //	if (GUILayout.Button("On/off gyro controller: " + gyroEnabled, GUILayout.Height(100)))
    //	{
    //		if (gyroEnabled)
    //		{
    //			DetachGyro();
    //		}
    //		else
    //		{
    //			AttachGyro();
    //		}
    //	}

    //	if (GUILayout.Button("Update gyro calibration (Horizontal only)", GUILayout.Height(80)))
    //	{
    //		UpdateCalibration(true);
    //	}

    //	if (GUILayout.Button("Update camera base rotation (Horizontal only)", GUILayout.Height(80)))
    //	{
    //		UpdateCameraBaseRotation(true);
    //	}

    //	if (GUILayout.Button("Reset base orientation", GUILayout.Height(80)))
    //	{
    //		ResetBaseOrientation();
    //	}

    //	if (GUILayout.Button("Reset camera rotation", GUILayout.Height(80)))
    //	{
    //		transform.rotation = Quaternion.identity;
    //	}
    //}

    #endregion

    #region [Public methods]

    /// <summary>
    /// Attaches gyro controller to the transform.
    /// </summary>
    /// 
    public void ResetOrient() {
        referanceRotation = Camera.main.GetComponentInParent<Transform>().rotation;
    }

    private void AttachGyro()
    {
        gyroEnabled = true;
    }


    private void DetachGyro()
    {
        gyroEnabled = false;
    }

    #endregion

    #region [Private methods]

    private void UpdateCalibration(bool onlyHorizontal)
    {
        if (onlyHorizontal)
        {
            var fw = (BaseSDK.GetQuaternion()) * (-Vector3.forward);
            fw.z = 0;
            if (fw == Vector3.zero)
            {
                calibration = Quaternion.identity;
            }
            else
            {
                calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
            }
        }
        else
        {
            calibration = BaseSDK.GetQuaternion();
        }
    }

    /// <summary>
    /// Update the camera base rotation.
    /// </summary>
    /// <param name='onlyHorizontal'>
    /// Only y rotation.
    /// </param>
    private void UpdateCameraBaseRotation(bool onlyHorizontal)
    {
        if (onlyHorizontal)
        {
            var fw = transform.forward;
            fw.y = 0;
            if (fw == Vector3.zero)
            {
                cameraBase = Quaternion.identity;
            }
            else
            {
                cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
            }
        }
        else
        {
            cameraBase = transform.rotation;
        }
    }

    private static Quaternion ConvertRotation(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
    private static Quaternion ConvertRotation2(Quaternion q) {
        return new Quaternion(q.x, q.y, q.z, q.w);
    }

    /// <summary>
    /// Gets the rot fix for different orientations.
    /// </summary>
    /// <returns>
    /// The rot fix.
    /// </returns>
    private Quaternion GetRotFix()
    {
#if UNITY_3_5
		if (Screen.orientation == ScreenOrientation.Portrait)
			return Quaternion.identity;
		
		if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.Landscape)
			return landscapeLeft;
				
		if (Screen.orientation == ScreenOrientation.LandscapeRight)
			return landscapeRight;
				
		if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
			return upsideDown;
		return Quaternion.identity;
#else
        return Quaternion.identity;
#endif
    }

    /// <summary>
    /// Recalculates reference system.
    /// </summary>
    private void ResetBaseOrientation()
    {
        baseOrientationRotationFix = GetRotFix();
        baseOrientation = baseOrientationRotationFix * baseIdentity;
    }

    /// <summary>
    /// Recalculates reference rotation.
    /// </summary>
    private void RecalculateReferenceRotation()
    {
        referanceRotation = Quaternion.Inverse(baseOrientation) * Quaternion.Inverse(calibration);
    }

    #endregion
}
