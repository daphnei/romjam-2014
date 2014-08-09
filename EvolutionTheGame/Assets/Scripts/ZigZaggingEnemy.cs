using UnityEngine;
using System.Collections;

public class ZigZaggingEnemy : MonoBehaviour {
	float fMagnitude = 0.75f;
	Vector3 v3Axis = new Vector3(0f, 1f, 0);
	
	void Start() {
		v3Axis.Normalize();
	}
	
	void Update () {
		transform.localPosition = v3Axis * Mathf.Sin (Time.time) * fMagnitude;
	}
}
