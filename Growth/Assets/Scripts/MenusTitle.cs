﻿using UnityEngine;
using System.Collections;

public class MenusTitle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<GUIText>().fontSize = 64;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
		{
			Application.LoadLevel("SampleLevel");
		}	
	}
}
