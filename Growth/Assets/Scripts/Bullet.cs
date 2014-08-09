using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public Vector2 localVelocity;

	// Update is called once per frame
	void Update () {
		this.transform.localPosition += (this.localVelocity * Time.deltaTime).ToVector3();
	}
}
