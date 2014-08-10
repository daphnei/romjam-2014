using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PulseController : SceneSingleton<PulseController> {

	public AudioSource song;
	public float bpm;

	private List<Pulser> pulsers = new List<Pulser>();
	private int samplesElapsed = 0;
	private int lastSamples = 0;

	public float slowestTime = 1f;
	public float fastestTime = 1.2f;
	public float songLerpSpeed = 0.012f;

	// Use this for initialization
	void Awake () {
		pulsers = this.GetComponentsInChildren<Pulser>().ToList();

		// nobody likes Newgrounds music
//		song.volume = 0;
		song.Play();
	}
	
	// Update is called once per frame
	void Update () {
		// song restarted
		if (song.timeSamples < lastSamples) {
			lastSamples = 0;
		}

		song.pitch = Mathf.MoveTowards(song.pitch, Mathf.Clamp(Time.timeScale, slowestTime, fastestTime), songLerpSpeed);

		samplesElapsed += song.timeSamples - lastSamples;
		lastSamples = song.timeSamples;

		if (Input.GetKeyDown(KeyCode.Space)) {
			PulseAll();
		}

		if (samplesElapsed > (60 / bpm) * 41800) {
			samplesElapsed = 0;
			PulseAll();
		}
	}

	void PulseAll() {
		foreach (Pulser pulser in this.pulsers) {
			pulser.Pulse();
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
