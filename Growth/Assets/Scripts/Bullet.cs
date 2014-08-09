using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public Vector2 localVelocity;

	// Update is called once per frame
	void Update () {
		this.transform.localPosition += (this.localVelocity * Time.deltaTime).ToVector3();

		Vector3 viewportPoint = Camera.main.WorldToViewportPoint(this.transform.position);
		if (viewportPoint.x < -0.1f || viewportPoint.x > 1.1f ||
		    viewportPoint.y < -0.1f || viewportPoint.y > 1.1f) {
			GameObject.Destroy(this.gameObject);
		}
	}
}
