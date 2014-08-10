using UnityEngine;
using System.Collections;

public class BulletCollider : MonoBehaviour {

	public Bullet bullet;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<FreeNutrient>() != null) {
			FreeNutrient nutrient = other.GetComponent<FreeNutrient>();
			if (nutrient.Color == bullet.Color) {
				Destroy(other.gameObject);
			
				World.Instance.player.AddNutrient();
			}

		} else if (other.GetComponent<Enemy>() != null) {
			Destroy(other.gameObject);
		}
	}
}
