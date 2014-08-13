using UnityEngine;
using System.Collections;

public class NutrientAnimator : Pulser {

	public float scaleWander = 0.05f;
	public float pulseLength = 0.1f;
	public float pulseScale = 0.5f;
	public float jitterweight = 0.2f;

	public bool fadeOut = false;
	private float fadeSpeed = 2f;
	private float fadeAmount = 0f;

	public bool FadedOut {
		get { return this.fadeAmount >= 1f; }
	}

	private NutrientColor color;
	public NutrientColor nutColor {
		get { return color; }
		set {
			color = value;
			kulur = value.ColorValue();
		}
	}

	private Color kulur {
		set {
			if (this.ring != null)
				this.ring.renderer.material.color = value;

			if (this.core != null)
				this.core.renderer.material.color = value;
			if (this.parts != null)
				parts.startColor = value;
		}
	}
	
	private float ringScaleInitial;
	private float coreScaleIntitial;

	private float pulseCount;

	public int NUMPARTICLES = 8;

	private Transform ring, core;
	private Light myLight;
	private ParticleSystem parts;

	// Use this for initialization
	protected override void Start() {
//		PulseController.Instance.AddPulser(this);
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
		pulseCount = 0;

		if (!this.fadeOut) {
			parts.Emit(NUMPARTICLES);
		}
	}

	void Update() {
		/*
		ParticleSystem.Particle[] ParticleList = new ParticleSystem.Particle[parts.particleCount];
		parts.GetParticles(ParticleList);

		for (int i = 0; i < ParticleList.Length; ++i) {
			float LifeProcentage = (ParticleList[i].lifetime / ParticleList[i].startLifetime);
			ParticleList[i].color = Color.Lerp( new Color(1,1,1,0), this.color.ColorValue(), LifeProcentage);
		}

		parts.SetParticles(ParticleList, parts.particleCount);
		*/

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

		if (this.fadeOut) {
			this.fadeAmount = this.fadeAmount + Time.deltaTime;
			kulur = Color.Lerp(
				this.color.ColorValue(), new Color(1,1,1, 0), 
				Easing.easeSin(  Mathf.Min(this.fadeAmount/this.fadeSpeed, 1) ) 
			);
			if (this.fadeAmount >= this.fadeSpeed) {
				Destroy(this.gameObject);
			}
		}
	}
}
