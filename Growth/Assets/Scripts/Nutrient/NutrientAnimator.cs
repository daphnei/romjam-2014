using UnityEngine;
using System.Collections;

public class NutrientAnimator : Pulser {

	public float scaleWander = 0.05f;
	public float pulseLength = 0.1f;
	public float pulseScale = 0.5f;
	public float jitterweight = 0.2f;
	
	private float ringScaleInitial;
	private float coreScaleIntitial;

	private float pulseCount;

	private Transform ring, core;
	private Light myLight;
	private ParticleSystem parts;

	// Use this for initialization
	protected override void  Start() {
		base.Start();
		core = this.transform.FindChild("core");
		ring = this.transform.FindChild("ring");
		myLight = this.GetComponent<Light>();
		ringScaleInitial = ring.transform.localScale.x;
		coreScaleIntitial = core.transform.localScale.x;

		parts = this.GetComponent<ParticleSystem>();

		pulseCount = pulseLength;
	}

	override public void Pulse() {
//		Debug.Log("!");
		pulseCount = 0;
		parts.Emit(20);
	}

	// Update is called once per frame
	void Update() {

		float pulse = 1;
		if (pulseCount < pulseLength) {
			pulseCount = pulseCount + Time.deltaTime;
			pulse = 1 + pulseScale * Easing.easeSinInv(pulseCount / pulseLength);
		}

		float d = 1f;
		d = Random.Range((1 - scaleWander), (1 + scaleWander));
		this.ring.localScale = this.ring.localScale * (1 - jitterweight) +
								new Vector3(pulse * pulse * ringScaleInitial * d, pulse * pulse * ringScaleInitial * d, 0) * jitterweight;
		d = Random.Range((1 - scaleWander), (1 + scaleWander));
 		this.core.localScale = this.core.localScale  * (1 - jitterweight) + 
								new Vector3(pulse * coreScaleIntitial * d,
											pulse * coreScaleIntitial * d,
											0) * jitterweight;

		myLight.intensity = this.core.localScale.x;
		myLight.range = this.ring.localScale.x;
	}
}
