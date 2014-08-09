using UnityEngine;
using System.Collections;

public class BackgroundCycler : MonoBehaviour {

	public float frequency = 0.1f;
	public int redMax = 255;
	public int greenMax = 255;
	public int blueMax = 255;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.backgroundColor = new Color(
			Mathf.Min(redMax / 255f, Mathf.Sin(frequency*Time.timeSinceLevelLoad) * 0.5f + 0.5f),
			Mathf.Min(greenMax / 255f, Mathf.Sin(frequency*Time.timeSinceLevelLoad + 2) * 0.5f + 0.5f),
			Mathf.Min(blueMax / 255f, Mathf.Sin(frequency*Time.timeSinceLevelLoad + 4) * 0.5f + 0.5f)
		);
	}
}
