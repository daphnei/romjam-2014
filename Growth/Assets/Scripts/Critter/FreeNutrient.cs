using UnityEngine;
using System;
using System.Collections;

public class FreeNutrient : Critter {

	NutrientAnimator animatorObj;

	// ugly hack for setting color after animator initialized
	private bool firstUpdate = true;

	protected virtual void Awake() {
		this.animatorObj = this.GetComponent<NutrientAnimator>();
	}

	// Update is called once per frame
	override public void DoUpdate () {
		base.DoUpdate();

		if (firstUpdate) {
			Array values = Enum.GetValues(typeof(NutrientColor));
			animatorObj.Color = (NutrientColor)values.GetValue(UnityEngine.Random.Range(0, values.Length));

			firstUpdate = false;
		}

		Vector3 positionOfPlayer = World.Instance.player.transform.position;
		
		//Move the enemy toward the center. This should maybe go faster as the enemy gets closer?
		Vector3 directionToPlayer = -(this.transform.position - positionOfPlayer);
		directionToPlayer.Normalize();
		this.transform.position += Time.deltaTime * (directionToPlayer * 1.5f);
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
