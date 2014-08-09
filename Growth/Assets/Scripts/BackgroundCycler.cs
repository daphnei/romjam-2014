using UnityEngine;
using System.Collections;

public class BackgroundCycler : Pulser {

	public float frequency = 3f;
	public float cycleDistance = 1f;

	public int redMax = 200;
	public int greenMax = 50;
	public int blueMax = 200;

	private float time;
	private float nextTime;

	protected override void Start() {
		PulseController.Instance.AddPulser(this);
		time = 0f;
		nextTime = 0f;
	}

	public override void Pulse()
	{
		base.Pulse();

		nextTime += 0.25f;
	}
	
	// Update is called once per frame
	void Update () {
		time = Mathf.MoveTowards(time, nextTime, 0.01f);

		Camera.main.backgroundColor = new Color(
			Mathf.Min(redMax / 255f, Mathf.Sin(frequency*time) * 0.5f + 0.5f),
			Mathf.Min(greenMax / 255f, Mathf.Sin(frequency*time + 2) * 0.5f + 0.5f),
			Mathf.Min(blueMax / 255f, Mathf.Sin(frequency*time + 4) * 0.5f + 0.5f)
		);
	}
}
