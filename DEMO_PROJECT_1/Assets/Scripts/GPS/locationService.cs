using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class locationService : MonoBehaviour {
	[SerializeField]
	private Text  textLat;
	[SerializeField]
	private Text  textLon;
	public GameObject items2;
	public float latGps {get;set;}
	public float lonGps {get; set;}
	const long offset = 16000;
	public float offsetpositionx = 4.5f;
	public float offsetpositiony = 4.5f;
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
		foreach (Transform it in items2.transform){
			GameObject item2 = it.gameObject;
			/*a = DrawCubeX(item2.GetComponent<hotpoint>().Lon, TileToWorldPos(x, y, zoom-1).X, TileToWorldPos(x + 1, y, zoom-1).X);
					b = DrawCubeY(item2.GetComponent<hotpoint>().Lat, TileToWorldPos(x, y + 1, zoom-1).Y, TileToWorldPos(x, y, zoom-1).Y);
					if (a > 1.0f || a < 0.0f || b < 0.0f || b > 1.0f) {
						a = -9000.0f;
						b = -9000.0f;
					}*/
			item2.transform.localPosition = Quaternion.AngleAxis((lonGps-item2.GetComponent<hotpoint>().Lon)*offset, -Vector3.up) * Quaternion.AngleAxis((latGps-item2.GetComponent<hotpoint>().Lat)*offset, -Vector3.right) * new Vector3(0,0,1);
			item2.transform.localPosition = new Vector3((float)item2.transform.localPosition.x*offsetpositionx, (float)item2.transform.localPosition.y*offsetpositiony, 0.0f);
			//item.transform.position = new Vector3((float)a, (float)b, -0.2f);
		}
	}
}