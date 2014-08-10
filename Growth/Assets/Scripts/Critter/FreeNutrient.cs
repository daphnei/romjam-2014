using UnityEngine;
using System.Collections;

public class FreeNutrient : Critter {

	NutrientAnimator animatorObj;
	public float speed;

	override protected void Start() {;
		this.animatorObj = this.GetComponent<NutrientAnimator>();
		base.Start();
	}

	public override void DoStart() {
		base.DoStart();
	}

	// Update is called once per frame
	override public void DoUpdate () {
		base.DoUpdate();

		Vector3 positionOfPlayer = World.Instance.player.transform.position;
		
		//Move the enemy toward the center. This should maybe go faster as the enemy gets closer?
		Vector3 directionToPlayer = -(this.transform.position - positionOfPlayer);
		directionToPlayer.Normalize();
		this.transform.position += Time.deltaTime * (directionToPlayer * this.speed);
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
