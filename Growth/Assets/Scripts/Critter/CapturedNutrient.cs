using UnityEngine;
using System.Collections;

public class CapturedNutrient : Pulser {

	NutrientAnimator animatorObj;

	// Use this for initialization
	void Start () {
		this.animatorObj = this.GetComponent<NutrientAnimator>();
		base.Start();
	}

	override public void Pulse() {
		//See how it looks without pulsing.
		//this.animatorObj.Pulse();
	}
}
