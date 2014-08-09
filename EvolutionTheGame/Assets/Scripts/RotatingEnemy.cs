using UnityEngine;
using System.Collections;

public class RotatingEnemy : Enemy {
	private bool rotateClockwise = true;

	/**
	 * How many degrees to rotate each frame
	 */
	public float rotationSpeed = 1;

	// Use this for initialization
	void Start () {
		this.rotateClockwise = Random.value >= 0.5f;

		//Debug.Log(this.rigidbody2D.position);
	}
	
	// Update is called once per frame
	void Update () {
		DoUpdate();
	}

	override public void DoUpdate()
	{
		base.DoUpdate();
	
		Vector2 positionOfPlayer = World.GetInstance().player.transform.position;

		//Move the enemy at the same speed no matter how close the enmy is to the rotation origin.
		float radius = Mathf.Abs((this.rigidbody2D.position - positionOfPlayer).magnitude);
		float circumference = 2 * Mathf.PI * radius;

		//Rotate the enemy.
		this.transform.RotateAround(positionOfPlayer, Vector3.forward, (this.rotateClockwise ? 1 : -1) * circumference / 70f);

		//Move the enemy toward the center. This should maybe go faster as the enemy gets closer?
		Vector3 directionToPlayer = -(this.rigidbody2D.position - positionOfPlayer);
		directionToPlayer.Normalize();
		this.transform.position += (directionToPlayer / 40);
	}
}
