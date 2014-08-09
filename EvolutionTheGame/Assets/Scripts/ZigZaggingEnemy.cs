using UnityEngine;
using System.Collections;

public class ZigZaggingEnemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.rigidbody2D.velocity = new Vector2(1, 0);
		this.transform.rotation = Quaternion.AngleAxis(20, Vector2.one);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
