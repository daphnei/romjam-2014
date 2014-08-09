using UnityEngine;
using System.Collections;

public class BulletCollider : MonoBehaviour {

	public Bullet bullet;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<FreeNutrient>() != null) {
			Destroy(other.gameObject);
		} else if (other.GetComponent<Enemy>() != null) {
			Destroy(other.gameObject);
		}
	}
}
