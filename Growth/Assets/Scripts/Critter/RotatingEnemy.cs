using UnityEngine;
using System.Collections;

public class RotatingEnemy : Critter {
	private bool rotateClockwise = true;

	/**
	 * How many degrees to rotate each frame
	 */
	public float rotationSpeed = 1.1f;

	public float minSpeed = 0.5f;
	public float maxSpeed = 1.1f;

	private float timeToGetToPlayer;

	// Use this for initialization
	public override void DoStart () {
		base.DoStart();

		this.timeToGetToPlayer = Random.Range(minSpeed, maxSpeed);

		//Debug.Log(this.rigidbody2D.position);
	}

	override public void DoUpdate()
	{
		base.DoUpdate();
		//Debug.Log(Time.deltaTime);
		Vector3 positionOfPlayer = World.Instance.player.transform.position;

		//Move the enemy at the same speed no matter how close the enmy is to the rotation origin.
		//TODO: This is prob unecessary and stupid.
		float radius = Mathf.Abs((this.transform.position - positionOfPlayer).magnitude);
		float circumference = 2 * Mathf.PI * radius;

		//Rotate the enemy.
		this.transform.RotateAround(positionOfPlayer, Vector3.forward,
		                            Time.deltaTime * (this.rotateClockwise ? -1 : 1) * (circumference / rotationSpeed));

		//Move the enemy toward the center. This should maybe go faster as the enemy gets closer?
		Vector3 directionToPlayer = -(this.transform.position - positionOfPlayer);
		directionToPlayer.Normalize();
		this.transform.position += Time.deltaTime * (directionToPlayer / timeToGetToPlayer);
	}

	//Only start these enemies on the long side because they don't work well starting on the short side.
	override protected void ChooseSpawnPoint()
	{
		//How far off screen to do initial position;
		int d = 3;

		bool topOrBottom = Random.value >= 0.5;
		bool leftOrRight = Random.value >= 0.5;

		//Choose a spawn point that is close to one of the corners.
		float xSpawnPos = leftOrRight ?
			Random.Range(0, 0.25f * Camera.main.pixelWidth) : 
			Random.Range(0.75f * Camera.main.pixelWidth, Camera.main.pixelWidth);

		//Choose a spawn point on either the top or the bottom.
		float ySpawnPos = topOrBottom ? -d : Camera.main.pixelHeight + d;

		Vector2 spawnPoint = new Vector2( xSpawnPos, ySpawnPos);
		spawnPoint = Camera.main.ScreenToWorldPoint(spawnPoint);

		this.rotateClockwise = (topOrBottom && leftOrRight) || (!topOrBottom && !leftOrRight);

		this.transform.position = spawnPoint;
	}
}
