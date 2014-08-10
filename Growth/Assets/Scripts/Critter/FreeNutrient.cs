﻿using UnityEngine;
using System;
using System.Collections;

public class FreeNutrient : Critter {

	NutrientAnimator animatorObj;
	public float speed;

	// ugly hack for setting color after animator initialized
	private bool firstUpdate = true;

	// change direction
	public int movementSign = 1;

	protected virtual void Awake() {
		this.animatorObj = this.GetComponent<NutrientAnimator>();
	}

	public NutrientColor Color {
		get { return animatorObj.Color; }
	}

	public override void DoStart() {
		base.DoStart();
	}

	// Update is called once per frame
	override public void DoUpdate () {
		base.DoUpdate();

		if (firstUpdate) {
			animatorObj.Color = randomColor();

			firstUpdate = false;
		}

		Vector3 positionOfPlayer = World.Instance.player.transform.position;
		
		//Move the enemy toward the center. This should maybe go faster as the enemy gets closer?
		Vector3 directionToPlayer = -(this.transform.position - positionOfPlayer);
		directionToPlayer.Normalize();
		this.transform.position += Time.deltaTime * (directionToPlayer * this.speed) * movementSign;
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

	public static NutrientColor randomColor()
	{
		Array values = Enum.GetValues(typeof(NutrientColor));
		int possibleColors = Mathf.Min(values.Length, World.Instance.player.polygon.numsides);
		NutrientColor color = (NutrientColor)values.GetValue(UnityEngine.Random.Range(0, possibleColors));

		return color;
	}
}
