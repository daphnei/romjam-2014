using UnityEngine;
using System.Collections;

public class BulletCollider : MonoBehaviour {

	public Bullet bullet;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<FreeNutrient>() != null) {
			FreeNutrient nutrient = other.GetComponent<FreeNutrient>();
			nutrient.GetComponent<CircleCollider2D>().enabled = false;

			if (nutrient.Color == bullet.Color) {
				//When it gets to the center, a captured nutrient will be added.
			}
			else {
				nutrient.movementSign = -1;
				nutrient.PrettyKill();
				World.Instance.player.RemoveNutrient();
			}

		} else if (other.GetComponent<Enemy>() != null) {
			Destroy(other.gameObject);
		}
	}
}
