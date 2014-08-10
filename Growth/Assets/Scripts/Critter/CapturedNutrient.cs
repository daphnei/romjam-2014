using UnityEngine;
using System.Collections;

public class CapturedNutrient : Pulser {

	NutrientAnimator animatorObj;

	bool firstUpdate = true;

	protected override void Start() {
		this.animatorObj = this.GetComponent<NutrientAnimator>();
		base.Start();
	}

	void Update()
	{
		if (firstUpdate) {
			animatorObj.nutColor = animatorObj.nutColor;
			
			firstUpdate = false;
		}
	}

	override public void Pulse() {
		//See how it looks without pulsing.
		//this.animatorObj.Pulse();
	}
}
