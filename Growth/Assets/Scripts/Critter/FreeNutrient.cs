using UnityEngine;
using System;
using System.Collections;

public class FreeNutrient : Critter {

	NutrientAnimator animatorObj;
	public Timeline timeline;
	public TimelineEntry timelineEntry;

	// ugly hack for setting color after animator initialized
	private bool firstUpdate = true;

	protected virtual void Awake() {
		this.animatorObj = this.GetComponent<NutrientAnimator>();
	}

	public override void DoStart() {
		base.DoStart();
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
		Vector3 dirPlayerToMe = this.transform.position - positionOfPlayer;
		dirPlayerToMe.Normalize();

		//Debug.Log("time " + this.timelineEntry.PercentBetweenSpawnAndHit(this.timeline));
		this.transform.position = positionOfPlayer +
			(dirPlayerToMe * (Player.PLAYER_RADIUS + timelineEntry.PercentBetweenSpawnAndHit(this.timeline) * timelineEntry.spawnDistance));

		//base.HitThePlayer();
		//World.Instance.player.AddNutrient();
	}

	override public void Pulse() {
//		Debug.Log(this.animatorObj);
		this.animatorObj.Pulse();
	}
}
