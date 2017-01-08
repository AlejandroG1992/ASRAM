using UnityEngine;
using System.Collections;

public class Gyroscope : MonoBehaviour 
{
	private Compass compass;
	private UiManager uiManager;


	private const float lowPassFilterFactor = 0.2f;

	private readonly Quaternion baseIdentity = Quaternion.Euler(90, 0, 0);

	private Quaternion cameraBase = Quaternion.identity;
	private Quaternion calibration = Quaternion.identity;
	private Quaternion baseOrientation = Quaternion.Euler(90, 0, 0);
	private Quaternion baseOrientationRotationFix = Quaternion.identity;
	private Quaternion referanceRotation = Quaternion.identity;

	// Called when script is loaded.
	void Awake()
	{
		compass = gameObject.GetComponent<Compass>();
		uiManager = GameObject.FindGameObjectWithTag("Ui").GetComponent<UiManager>();
	}

	// Use this for initialization
	void Start () 
	{
		Debug.Log("Gyroscope");
		Input.gyro.enabled = true;

		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + compass.Heading, 0);

		InitializeGyro();
	}

	/// <summary>
	/// Initializes the gyroscope.
	/// </summary>
	private void InitializeGyro()
	{
		ResetBaseOrientation();
		UpdateCalibration(true);
		UpdateCameraBaseRotation(true);
		RecalculateReferenceRotation();
	}

	/// <summary>
	/// Recalculates reference system.
	/// </summary>
	private void ResetBaseOrientation()
	{
		baseOrientation = baseOrientationRotationFix * baseIdentity;
	}

	/// <summary>
	/// Recalculates reference rotation.
	/// </summary>
	private void RecalculateReferenceRotation()
	{
		referanceRotation = Quaternion.Inverse(baseOrientation) * Quaternion.Inverse(calibration);
	}

	/// <summary>
	/// Update the gyro calibration.
	/// </summary>
	private void UpdateCalibration(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = (Input.gyro.attitude) * (-Vector3.forward);
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
			calibration = Input.gyro.attitude;
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

	/// <summary>
	/// Converts the rotation from right handed to left handed.
	/// </summary>
	/// <returns>
	/// The result rotation.
	/// </returns>
	/// <param name='q'>
	/// The rotation to convert.
	/// </param>
	private static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);

	}
	// Update is called once per frame
	void Update () 
	{
		//transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
		transform.rotation = Quaternion.Slerp(transform.rotation,
			cameraBase * (ConvertRotation(referanceRotation * Input.gyro.attitude) * Quaternion.identity), lowPassFilterFactor);
		//uiManager.WriteCompassValue(transform.eulerAngles.y.ToString());

	}
}