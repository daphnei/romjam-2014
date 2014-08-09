using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PulseController : SceneSingleton<PulseController> {

	public AudioSource song;
	public float bpm;

	private List<Pulser> pulsers;
	private int samplesElapsed = 0;
	private int lastSamples = 0;

	// Use this for initialization
	void Start () {
		pulsers = this.GetComponentsInChildren<Pulser>().ToList();
		song.Play();
	}
	
	// Update is called once per frame
	void Update () {
		samplesElapsed += song.timeSamples - lastSamples;
		lastSamples = song.timeSamples;

		if (samplesElapsed > (bpm / 60.0f) * 41800) {
			samplesElapsed = 0;
	
			foreach (Pulser pulser in this.pulsers) {
				pulser.Pulse();
			}
		}
	}

	public void AddPulser(Pulser pulser) {
		if (!this.pulsers.Contains(pulser))
			this.pulsers.Add(pulser);
	}

	public void RemovePulser(Pulser p) {
		this.pulsers.Remove(p);	
	}
}
