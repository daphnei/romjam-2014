﻿using UnityEngine;
using System;
using System.Collections;

public class FreeNutrient : Critter {

	public NutrientAnimator animatorObj;
	public Timeline timeline;
	public TimelineEntry timelineEntry;

	// ugly hack for setting color after animator initialized
	private bool firstUpdate = true;

	// change direction
	public int movementSign = 1;

	protected virtual void Awake() {
		this.animatorObj = this.GetComponent<NutrientAnimator>();
	}

	public NutrientColor soonColor;

	public NutrientColor Color {
		get { return animatorObj.nutColor; }
	}

	public override void DoStart() {
		base.DoStart();

		Vector3 v = this.transform.position;
		v.z = - 10;
		this.transform.position = v;
	}

	// Update is called once per frame
	override public void DoUpdate() {
		base.DoUpdate();

		Vector3 viewportCoords = Camera.main.WorldToViewportPoint(this.transform.position);
		if (viewportCoords.x < -0.5f || viewportCoords.x > 1.5f || viewportCoords.y < -0.5f || viewportCoords.y > 1.5f) {
			GameObject.Destroy(this);	
		}

		if (firstUpdate) {
			animatorObj.nutColor = soonColor;

			firstUpdate = false;
		}

		//When close to the centure, delete and add a captured nutrient.
		if (Vector2.Distance(this.transform.position, Vector2.zero) < 0.1f)
		{
			World.Instance.player.AddNutrient(this.Color);

			Destroy (this.gameObject);

			return;
		}

		Vector3 positionOfPlayer = World.Instance.player.transform.position;
		if (this.movementSign == 1) {
			Vector3 dirPlayerToMe = this.transform.position - positionOfPlayer;
			dirPlayerToMe.Normalize();

			//Debug.Log("time " + this.timelineEntry.PercentBetweenSpawnAndHit(this.timeline));
			this.transform.position = positionOfPlayer +
				(dirPlayerToMe * (Player.PLAYER_RADIUS + timelineEntry.PercentBetweenSpawnAndHit(this.timeline) * timelineEntry.spawnDistance));
		} else {
			//Move the enemy toward the center. This should maybe go faster as the enemy gets closer?
			Vector3 directionToPlayer = -(this.transform.position - positionOfPlayer);
			directionToPlayer.Normalize();
			this.transform.position += Time.deltaTime * (directionToPlayer * this.timelineEntry.speed) * movementSign;
		}
	}

	override protected void HitThePlayer()
	{
		base.HitThePlayer();

		World.Instance.player.AddNutrient(this.Color);
	}

	override public void Pulse() {
//		Debug.Log(this.animatorObj);
		this.animatorObj.Pulse();
	}

	protected override void ChooseSpawnPoint() {
		
	}
}