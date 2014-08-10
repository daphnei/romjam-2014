using UnityEngine;
using System.Collections;

public class BulletCollider : MonoBehaviour {

	public Bullet bullet;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<FreeNutrient>() != null) {
			FreeNutrient nutrient = other.GetComponent<FreeNutrient>();
			if (nutrient.Color == bullet.Color) {

				//Don't delete the nutrient let it permeate.
				nutrient.GetComponent<CircleCollider2D>().enabled = false;

				//When it gets to the center, a captured nutrient will be added.
			} else {
				nutrient.movementSign = -1;

				World.Instance.player.RemoveNutrient();
			}

		} else if (other.GetComponent<Enemy>() != null) {
			Destroy(other.gameObject);
		}
	}
}
