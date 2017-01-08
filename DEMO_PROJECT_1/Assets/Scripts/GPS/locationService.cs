using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class locationService : MonoBehaviour {
	[SerializeField]
	private Text  textLat;
	[SerializeField]
	private Text  textLon;

	public float latGps {get;set;}
	public float lonGps {get; set;}

	IEnumerator Start()
	{
		Debug.Log("locationService");
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
			yield break;
		// Start service before querying location
		Input.location.Start();
		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			yield return new WaitForSeconds(1);
			maxWait--;
		}
		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			print("Timed out");
			yield break;
		}
		// Connectionhas failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			print("Unable to determine device location");
			yield break;
		}

		StartCoroutine(readData());
	}

	IEnumerator readData(){
		
			textLat.text = Input.location.lastData.latitude.ToString ();
			textLon.text = Input.location.lastData.longitude.ToString ();
			lonGps = Input.location.lastData.longitude;
			latGps = Input.location.lastData.latitude;
	

		yield return new WaitForSeconds(1);

	}
	void Update ()
	{
		textLat.text = Input.location.lastData.latitude.ToString ();
		textLon.text = Input.location.lastData.longitude.ToString ();
		lonGps = Input.location.lastData.longitude;
		latGps = Input.location.lastData.latitude;
	}
}