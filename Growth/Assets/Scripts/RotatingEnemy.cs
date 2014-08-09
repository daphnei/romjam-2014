using UnityEngine;
using System.Collections;

public class RotatingEnemy : Enemy {
	private bool rotateClockwise = true;

	/**
	 * How many degrees to rotate each frame
	 */
	public float rotationSpeed = 1;

	private float timeToGetToPlayer;

	// Use this for initialization
	public override void DoStart () {
		base.DoStart();

		this.rotateClockwise = Random.value >= 0.5f;

		this.timeToGetToPlayer = Random.Range(30, 70);

		//Debug.Log(this.rigidbody2D.position);
	}

	override public void DoUpdate()
	{
		base.DoUpdate();
	
		Vector3 positionOfPlayer = World.Instance.player.transform.position;

		//Move the enemy at the same speed no matter how close the enmy is to the rotation origin.
		float radius = Mathf.Abs((this.transform.position - positionOfPlayer).magnitude);
		float circumference = 2 * Mathf.PI * radius;

		//Rotate the enemy.
		this.transform.RotateAround(positionOfPlayer, Vector3.forward, (this.rotateClockwise ? 1 : -1) * circumference / 70f);

		//Move the enemy toward the center. This should maybe go faster as the enemy gets closer?
		Vector3 directionToPlayer = -(this.transform.position - positionOfPlayer);
		directionToPlayer.Normalize();
		this.transform.position += (directionToPlayer / timeToGetToPlayer);
	}

	//Only start these enemies on the long side because they don't work well starting on the short side.
	override protected void ChooseSpawnPoint()
	{
		//How far off screen to do initial position;
		int d = 2;
		
		//Choose a spawn point on one of the edges of the viewport.
		Vector2 spawnPoint = new Vector2(
				Random.Range(0, Camera.main.pixelWidth),
				Random.value >= 0.5 ? -d : Camera.main.pixelHeight + d);
		spawnPoint = Camera.main.ScreenToWorldPoint(spawnPoint);
		
		this.transform.position = spawnPoint;
	}
}
