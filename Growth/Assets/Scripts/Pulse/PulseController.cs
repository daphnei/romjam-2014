using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PulseController : SceneSingleton<PulseController> {

	public AudioSource[] songs;
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
		foreach (AudioSource song in songs){
			song.volume = 0;
			song.Play();
		}
		songs[0].volume = 1;
	}
	
	// Update is called once per frame
	void Update () {
		// song restarted
		if (songs[0].timeSamples < lastSamples) {
			lastSamples = 0;
		}

		//song.pitch = Mathf.MoveTowards(song.pitch, Mathf.Clamp(Time.timeScale, slowestTime, fastestTime), songLerpSpeed);

		samplesElapsed += songs[0].timeSamples - lastSamples;
		lastSamples = songs[0].timeSamples;

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

	public void ChangeNumLayers(int numlayers) {
		for (int i = 0; i < songs.Length; i++) {
			if (i < numlayers) {
				songs[i].volume = 0f;
			}
		}
		songs[ Mathf.Min(numlayers, songs.Length - 1)].volume = 1f;
	}

	public void AddPulser(Pulser pulser) {
		if (!this.pulsers.Contains(pulser))
			this.pulsers.Add(pulser);
	}

	public void RemovePulser(Pulser p) {
		this.pulsers.Remove(p);	
	}
}
