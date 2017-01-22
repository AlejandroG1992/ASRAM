using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hotpoint : MonoBehaviour {

	public float Lat;
	public float Lon;
	public string description;
	public string horario;

	[SerializeField]
	private Text textBoxPointer;
	[SerializeField]
	private Text textBoxPointer2;
	public Image infoImage;
	public Sprite foto;
	// Use this for initialization

	void OnMouseDown () {
		textBoxPointer.text= description;
		textBoxPointer2.text= horario;
		infoImage.sprite = foto;
	}
}