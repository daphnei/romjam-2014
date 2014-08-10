using UnityEngine;
using System.Collections;

public class FreeNutrient : Critter {

	NutrientAnimator animatorObj;

	override protected void Start() {;
		this.animatorObj = this.GetComponent<NutrientAnimator>();
		base.Start();
	}

	// Update is called once per frame
	override public void DoUpdate () {
		base.DoUpdate();

		Vector3 positionOfPlayer = World.Instance.player.transform.position;
		
		//Move the enemy toward the center. This should maybe go faster as the enemy gets closer?
		Vector3 directionToPlayer = -(this.transform.position - positionOfPlayer);
		directionToPlayer.Normalize();
		this.transform.position += Time.deltaTime * (directionToPlayer * 4f);
	}

	override protected void HitThePlayer()
	{
		base.HitThePlayer();
		World.Instance.player.AddNutrient();
	}

	override public void Pulse() {
//		Debug.Log(this.animatorObj);
		this.animatorObj.Pulse();
	}
}
