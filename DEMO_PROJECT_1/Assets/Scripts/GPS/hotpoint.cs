using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hotpoint : MonoBehaviour {

	public float Lat;
	public float Lon;
	public string description;

	[SerializeField]
	private Text textBoxPointer;
	public Image infoImage;
	public Sprite foto;
	// Use this for initialization

	void OnMouseDown () {
		textBoxPointer.text= description;
		infoImage.sprite = foto;
	}
}