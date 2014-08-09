using UnityEngine;
using System.Collections;

public class Nutrient : Critter {
	
	// Update is called once per frame
	override public void DoUpdate () {
		base.DoUpdate();

		Vector3 positionOfPlayer = World.Instance.player.transform.position;
		
		//Move the enemy toward the center. This should maybe go faster as the enemy gets closer?
		Vector3 directionToPlayer = -(this.transform.position - positionOfPlayer);
		directionToPlayer.Normalize();
				this.transform.position += Time.deltaTime * (directionToPlayer * 1.5f);
	}
}
