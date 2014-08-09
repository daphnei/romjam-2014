using UnityEngine;
using System.Collections;

public class BackgroundCycler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.backgroundColor = new Color(
			Mathf.Sin(0.1f*Time.timeSinceLevelLoad) * 0.5f + 0.5f,
			Mathf.Sin(0.1f*Time.timeSinceLevelLoad + 2) * 0.5f + 0.5f,
			Mathf.Sin(0.1f*Time.timeSinceLevelLoad + 4) * 0.5f + 0.5f
		);
	}
}
