using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PulseController : MonoBehaviour {

	public AudioSource song;

	private Pulser[] pulsers;
	private int samplesElapsed = 0;
	private int lastSamples = 0;

	// Use this for initialization
	void Start () {
		pulsers = this.GetComponentsInChildren<Pulser>();
		song.Play();
	}
	
	// Update is called once per frame
	void Update () {
		samplesElapsed += song.timeSamples - lastSamples;
		lastSamples = song.timeSamples;

		if (samplesElapsed > 50000) {
			samplesElapsed = 0;

			foreach (Pulser pulser in this.pulsers) {
				pulser.Pulse();
			}
		}
	}
}
