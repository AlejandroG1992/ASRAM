using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Compass : MonoBehaviour
{
	/// <summary>
	/// The degrees to the North.
	/// </summary>
	const double offset = 30.0d;
	const int iMAX = 30;
	public float Heading { get; set; }

	/// <summary>
	/// Reference to the UI manager object.
	/// </summary>
	/// 
	public GameObject items2;
	private UiManager uiManager;
	private GameObject vision1;
	private GameObject vision2;
	private GameObject user;
	private GameObject map2;
	private double[] buffercos = new double[30];
	private double[] buffersin = new double[30];
	private static int i = 0;
	// Called when script is loaded.
	void Awake ()
	{
		uiManager = GameObject.FindGameObjectWithTag ("Ui").GetComponent<UiManager> ();
	}

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Compass");
		Input.compass.enabled = true;
		vision1 = GameObject.FindGameObjectWithTag ("vision1");
		vision2 = GameObject.FindGameObjectWithTag ("vision2");
		user = GameObject.FindGameObjectWithTag("user");
		map2 = GameObject.FindGameObjectWithTag("map2");
		vision1.transform.localPosition = new Vector3((float)0.5f, (float)0.5f, vision1.transform.localPosition.z);
	}

	// Update is called once per frame
	void Update ()
	{
		double meanValueAnglecos;
		double meanValueAnglesin;
		double meanValueAngle;
		Heading = Input.compass.trueHeading;

		//uiManager.WriteCompassValue (Heading.ToString ());

		if (i > iMAX - 1)
			i = 0;
		buffercos[i] = (double)System.Math.Cos (Heading * System.Math.PI / 180.0d);
		buffersin[i] = (double)System.Math.Sin (Heading * System.Math.PI / 180.0d);
		i++;
		meanValueAnglecos = 0.0d;
		meanValueAnglesin = 0.0d;
		meanValueAngle = 0.0d;
		for (int j = 0; j < iMAX; j++) {
			meanValueAnglecos += buffercos [j];
			meanValueAnglesin += buffersin [j];
		}
		meanValueAngle = (double)System.Math.Atan2 (meanValueAnglesin, meanValueAnglecos)*180.0d/System.Math.PI;	
		SetDirection (meanValueAngle);
		uiManager.WriteCompassValue (meanValueAngle.ToString ());

	}

	public void SetDirection (double angle)
	{
		double angle0, angle1, angle2, x, y;
		angle0 = 180.0d - angle;
		angle1 = angle0 - offset;
		angle2 = angle0 + offset;

		vision1.transform.localRotation = Quaternion.Euler (vision1.transform.localRotation.x, vision1.transform.localRotation.y, (float)angle1);
		vision2.transform.localRotation = Quaternion.Euler (vision2.transform.localRotation.x, vision2.transform.localRotation.y, (float)angle2);


		angle1 = angle0 - offset - 90.00d;
		angle2 = angle0 + offset - 90.00d;

		angle0 = 180.0d - angle0;
		map2.transform.localRotation = Quaternion.Euler ((float)45.0d, map2.transform.localRotation.y, (float)angle0);

		x = (double)vision1.transform.localScale.y * System.Math.Cos (angle1 * System.Math.PI / 180.0d) + user.transform.localPosition.x;
		y = (double)vision1.transform.localScale.y * System.Math.Sin (angle1 * System.Math.PI / 180.0d) + user.transform.localPosition.y;

		vision1.transform.localPosition = new Vector3((float)x, (float)y, vision1.transform.localPosition.z);

		x = (double)vision2.transform.localScale.y * System.Math.Cos (angle2 * System.Math.PI / 180.0d) + user.transform.localPosition.x;
		y = (double)vision2.transform.localScale.y * System.Math.Sin (angle2 * System.Math.PI / 180.0d) + user.transform.localPosition.y;

		vision2.transform.localPosition = new Vector3((float)x, (float)y, vision2.transform.localPosition.z);

		foreach (Transform it in items2.transform){
			GameObject item2 = it.gameObject;

			item2.transform.localRotation = Quaternion.Euler (item2.transform.localRotation.x, item2.transform.localRotation.y, item2.transform.localRotation.z + (float)angle0);

		}
	}
}
