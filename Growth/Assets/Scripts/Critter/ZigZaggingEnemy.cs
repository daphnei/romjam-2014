using UnityEngine;
using System.Collections;

public class ZigZaggingEnemy : Enemy {
	public float minSineMagnitude = 1;
	public float maxSineMagnitude = 1.5f;

	public float minSineFrequency = 5;
	public float maxSineFrequency = 10;
	
	private float sineMagnitude;
	private float sineFrequency;

	/**
	 * A bigger value means it will take longer to get to the player.
	 */
	private float speedToCenter = 2f;

	private Vector3 startingPos;

	public override void DoStart() {
		base.DoStart();

		startingPos = transform.position;

		this.sineFrequency = Random.Range(minSineFrequency, maxSineFrequency);
		this.sineMagnitude = Random.Range (minSineMagnitude, maxSineMagnitude);
	}
	
	override public void DoUpdate()
	{
		base.DoUpdate();

		Vector3 positionOfPlayer = World.Instance.player.transform.position;

		//Need to move toward the player.
		Vector3 directionToPlayer = -(this.transform.position - positionOfPlayer);

		//Sine movement should be perpendicular to the movement toward player.
		Vector3 perpendicularToPlayer = new Vector3(-directionToPlayer.y, directionToPlayer.x, 0);
		perpendicularToPlayer.Normalize();

		//Move along the sine
		this.transform.position = startingPos + (perpendicularToPlayer * Mathf.Sin (Time.time * sineFrequency) * sineMagnitude);

		//Also move toward the player.
		directionToPlayer.Normalize();
		var offset = Time.deltaTime * (directionToPlayer * speedToCenter);
		this.transform.position += offset;
		this.startingPos += offset;
	}
}
