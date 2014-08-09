using UnityEngine;
using System.Collections;

public class BackgroundCycler : Pulser {

	public float frequency = 0.1f;
	public float cycleMovementSpeed = 2f;

	public int redMax = 255;
	public int greenMax = 255;
	public int blueMax = 255;

	private float time;
	private float nextTime;

	protected override void Start() {
		PulseController.Instance.AddPulser(this);
		time = Time.timeSinceLevelLoad;
		nextTime = Time.timeSinceLevelLoad;
	}

	public override void Pulse ()
	{
		base.Pulse();

		nextTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		time = Mathf.MoveTowards(time, nextTime, cycleMovementSpeed);

		Camera.main.backgroundColor = new Color(
			Mathf.Min(redMax / 255f, Mathf.Sin(frequency*time) * 0.5f + 0.5f),
			Mathf.Min(greenMax / 255f, Mathf.Sin(frequency*time + 2) * 0.5f + 0.5f),
			Mathf.Min(blueMax / 255f, Mathf.Sin(frequency*time + 4) * 0.5f + 0.5f)
		);
	}
}
