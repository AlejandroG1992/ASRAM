using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class mapDownLoad : MonoBehaviour {
	public GameObject plane;
	public GameObject items;

	private float tileX;
	private float tileY;
	public float lat1;
	public float lon1;
	public bool fakegps = true;
	public int zoom= 17;
	// Use this for initialization

	private GameObject gpspointer;
	private GameObject user;

	public void Start () {
		Debug.Log("mapDownLoad");
		gpspointer = GameObject.FindGameObjectWithTag("gps");
		user = GameObject.FindGameObjectWithTag("user");
		//Debug.Log("map X pos"+user.transform.localPosition.x+"map Y pos"+user.transform.localPosition.y+"map Z pos"+user.transform.localPosition.z);
		user.transform.localPosition = new Vector3((float)0.5f, (float)0.5f, user.transform.localPosition.z);

		StartCoroutine(loadMAp());
	}

	public void WorldToTilePos(float lon, float lat, int zoom)
	{
		tileX = (float)((lon + 180.0f) / 360.0f * (1 << zoom));
		tileY = (float)((1.0f -Mathf.Log(Mathf.Tan(lat * Mathf.PI / 180.0f) +1.0f / Mathf.Cos(lat * Mathf.PI / 180.0f)) / Mathf.PI) / 2.0f * (1 << zoom));
	}

	IEnumerator loadMAp(){
		int loaded = 0;
		while (loaded == 0){
			if(!fakegps){
			lon1 = gpspointer.GetComponent<locationService>().lonGps;
			lat1 = gpspointer.GetComponent<locationService>().latGps;
			}
			if (lon1>0.0f){
				WorldToTilePos(lon1,lat1,zoom);
				string url = "http://a.tile.openstreetmap.org/"+zoom+"/"+Mathf.FloorToInt(tileX)+"/"+ Mathf.FloorToInt(tileY)+".png";
				WWW www = new WWW(url);
				yield return www;
				Texture2D texture = new Texture2D(2,2,TextureFormat.ARGB32, true);
				www.LoadImageIntoTexture(texture);
				plane.GetComponent<MeshRenderer>().material.mainTexture = texture;

				loaded =1;
				int y = Mathf.FloorToInt((float)tileY);
				int x = Mathf.FloorToInt((float)tileX);

				double a = DrawCubeX(lon1, TileToWorldPos(x, y, zoom).X, TileToWorldPos(x + 1, y, zoom).X);
				double b = DrawCubeY(lat1, TileToWorldPos(x, y + 1, zoom).Y, TileToWorldPos(x, y, zoom).Y);
				user.transform.localPosition = new Vector3((float)a-0.5f, (float)b-0.5f, user.transform.localPosition.z);
				//Debug.Log("map X pos"+user.transform.localPosition.x+"map Y pos"+user.transform.localPosition.y+"map Z pos"+user.transform.localPosition.z);

				foreach (Transform it in items.transform){
					GameObject item = it.gameObject;
					a = DrawCubeX(item.GetComponent<hotpoint>().Lon, TileToWorldPos(x, y, zoom).X, TileToWorldPos(x + 1, y, zoom).X);
					b = DrawCubeY(item.GetComponent<hotpoint>().Lat, TileToWorldPos(x, y + 1, zoom).Y, TileToWorldPos(x, y, zoom).Y);
					if (a > 1.0f || a < 0.0f || b < 0.0f || b > 1.0f) {
						a = -9000.0f;
						b = -9000.0f;
					}
					item.transform.localPosition = new Vector3((float)a-0.5f, (float)b-0.5f, item.transform.localPosition.z);
					//item.transform.position = new Vector3((float)a, (float)b, -0.2f);
				}


			}
			yield return new WaitForSeconds(2);
		}

		while (true){

			if(!fakegps){
				lon1 = gpspointer.GetComponent<locationService>().lonGps;
				lat1 = gpspointer.GetComponent<locationService>().latGps;
			}

			if (lon1>0.0f){
				//WorldToTilePos(lon1,lat1,zoom);
				int y = Mathf.FloorToInt((float)tileY);
				int x = Mathf.FloorToInt((float)tileX);

				double a = DrawCubeX(lon1, TileToWorldPos(x, y, zoom).X, TileToWorldPos(x + 1, y, zoom).X);
				double b = DrawCubeY(lat1, TileToWorldPos(x, y + 1, zoom).Y, TileToWorldPos(x, y, zoom).Y);
				user.transform.localPosition = new Vector3((float)a-0.5f, (float)b-0.5f, user.transform.localPosition.z);
				if (a > 1.0f || a < 0.0f || b < 0.0f || b > 1.0f) {
					WorldToTilePos(lon1,lat1,zoom);
					string url = "http://a.tile.openstreetmap.org/"+zoom+"/"+Mathf.FloorToInt(tileX)+"/"+ Mathf.FloorToInt(tileY)+".png";
					WWW www = new WWW(url);
					yield return www;
					Texture2D texture = new Texture2D(2,2,TextureFormat.ARGB32, true);
					www.LoadImageIntoTexture(texture);
					plane.GetComponent<MeshRenderer>().material.mainTexture = texture;


					foreach (Transform it in items.transform){
						GameObject item = it.gameObject;
						a = DrawCubeX(item.GetComponent<hotpoint>().Lon, TileToWorldPos(x, y, zoom).X, TileToWorldPos(x + 1, y, zoom).X);
						b = DrawCubeY(item.GetComponent<hotpoint>().Lat, TileToWorldPos(x, y + 1, zoom).Y, TileToWorldPos(x, y, zoom).Y);
						if (a > 1.0f || a < 0.0f || b < 0.0f || b > 1.0f) {
							a = -9000.0f;
							b = -9000.0f;
						}
						item.transform.localPosition = new Vector3((float)a-0.5f, (float)b-0.5f, item.transform.localPosition.z);
						//item.transform.position = new Vector3((float)a, (float)b, -0.2f);
					}
				}
			}

			yield return new WaitForSeconds(2);
		}

	}


	public double DrawCubeY(double targetLat, double minLat, double maxLat) {
		double pixelY = ((targetLat - minLat) / (maxLat - minLat)) * 1;
		return pixelY;
	}

	public double DrawCubeX(double targetLong, double minLong, double maxLong) {
		double pixelX = ((targetLong - minLong) / (maxLong - minLong)) * 1;
		return pixelX;
	}
	public struct Point {
		public double X;
		public double Y;
	}
	// devuelve la esquina superior izquierda del tile
	public Point TileToWorldPos(double tile_x, double tile_y, int zoom) {
		Point p = new Point();
		double n = System.Math.PI - ((2.0 * System.Math.PI * tile_y) / System.Math.Pow(2.0, zoom));

		p.X = ((tile_x / System.Math.Pow(2.0, zoom) * 360.0) - 180.0);
		p.Y = (180.0 / System.Math.PI * System.Math.Atan(System.Math.Sinh(n)));

		return p;
	}
}
