using UnityEngine;
using System.Collections;

public class RotatingEnemy : Enemy {

	/**
	 * This is a temporary hack until I sync up with the code that has the player.
	 */
	public static Vector2 locationOfPlayer = Vector2.zero;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.RotateAround(locationOfPlayer, Vector3.forward, 2);

		Vector3 directionToPlayer = -(this.rigidbody2D.position - locationOfPlayer);
		this.transform.position += (directionToPlayer / 200);
	}
}
