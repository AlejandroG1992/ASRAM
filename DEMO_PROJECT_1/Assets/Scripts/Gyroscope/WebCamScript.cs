﻿using UnityEngine;
using System.Collections;

public class WebCamScript : MonoBehaviour {


	// Use this for initialization
	void Start () {
		WebCamTexture webcamTexture = new WebCamTexture();
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = webcamTexture;
		webcamTexture.Play();
	}




}