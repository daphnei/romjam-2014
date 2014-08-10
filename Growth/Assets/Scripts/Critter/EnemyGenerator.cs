using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Timeline {

	public float timelimePosition { get { return Time.timeSinceLevelLoad - this.timelineStartPosition; } }
	public float timelineStartPosition = 0;
	int timelineIndex = 0;
	List<TimelineEntry> entries = new List<TimelineEntry>();

	public void RestartTimeline() {
		this.timelineStartPosition = Time.timeSinceLevelLoad - entries.First().spawnTime;
		this.timelineIndex = 0;
	}

	public IEnumerable<TimelineEntry> UpdateTimeline() {
		float timelinePos = this.timelimePosition;

		if (this.timelineIndex >= this.entries.Count) {
			yield break;
		}

		TimelineEntry nextEntry = this.entries[this.timelineIndex];
		if (timelinePos > nextEntry.spawnTime) {
			yield return nextEntry;
			this.timelineIndex++;
		}
	}

	public static Timeline GenerateTimeline(int lineLength, float timeStep) {
		Timeline timeline = new Timeline();
		TimelineEntry[] entries = new TimelineEntry[lineLength];
		int i = 0;
		while (i < entries.Length) {

			TimelineEntry entry = entries[i] = new TimelineEntry();
			entry.hitTime = i * timeStep;
			entry.speed = 5f;
			entry.spawnDistance = 6f;
			entry.angle = new Vector2(0, 1); //Random.insideUnitCircle.normalized;

			switch (UnityEngine.Random.Range(0, 5)) {
				case 0:
					entry.angle = new Vector2(0, 1);
					i++;
					break;
				case 1:
					entry.angle = new Vector2(0, -1);
					i++;
					break;
				case 2:
					entry.angle = new Vector2(1, 1);
					i++;
					if (i >= entries.Length) {
						break;
					}
					entry = entries[i] = new TimelineEntry();
					entry.hitTime = i * timeStep;
					entry.speed = 5f;
					entry.spawnDistance = 6f;
					entry.angle = new Vector2(1, 1);
					entry.colorSame = true;
					break;
				case 3:
					entry.angle = new Vector2(-1, 1);
					i++;
					if (i >= entries.Length) {
						break;
					}
					entry = entries[i] = new TimelineEntry();
					entry.hitTime = i * timeStep;
					entry.speed = 5f;
					entry.spawnDistance = 6f;
					entry.angle = new Vector2(-1, 1);
					entry.colorSame = true;
					break;
				case 4:
					entry.angle = new Vector2(1, 0);
					entry.speed *= 0.5f;
					i++;
					break;
				case 5:
					entry.angle = new Vector2(-1, 0);
					entry.speed *= 0.5f;
					i++;
					break;
			}

			if (UnityEngine.Random.Range(0, 3) == 0) {
				i += 1;
			} else {
				i += 2;
			}
		}
		timeline.entries = entries.Where(e => e != null).OrderBy<TimelineEntry, float>(e => e.spawnTime).ToList<TimelineEntry>();
		return timeline;
	}
}

public class TimelineEntry {
	public float hitTime;
	public float speed;
	public float spawnDistance;
	public Vector2 angle;
	public float spawnTime { get { return hitTime - spawnDistance / speed; } }
	public bool colorSame = false;

	public float PercentBetweenSpawnAndHit(Timeline t) {
		return (this.hitTime - t.timelimePosition) / (this.hitTime - this.spawnTime);
	}
}

public class EnemyGenerator : MonoBehaviour {

	public float minAttachRateInSeconds = 0.5f;
	public float maxAttachRateInSeconds = 3f;

	private float timeUntilSpawn;

	public GameObject[] enemies;
	public Timeline timeline;

	float timestep = 0.6f;
	public int timelength = 100;

	// Use this for initialization
	void Start () {
		this.timeline = Timeline.GenerateTimeline(this.timelength, this.timestep);
		this.timeline.RestartTimeline();
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilSpawn -= Time.deltaTime;

		foreach (TimelineEntry entry in this.timeline.UpdateTimeline()) {
			Debug.Log("spawning at " + entry.spawnTime + " (" + timeline.timelimePosition + ") to hit at " + entry.hitTime);
			this.SpawnEnemy(entry);
		}
	}

	NutrientColor lastColor = NutrientColor.Purple;

	void SpawnEnemy(TimelineEntry entry)
	{
		int enemyIndex = UnityEngine.Random.Range(0, enemies.Length);
		GameObject obj = Instantiate(enemies[enemyIndex]) as GameObject;
		Vector2 dir = entry.angle.normalized; //Random.insideUnitCircle.normalized;
		//Debug.Log(dir.magnitude.ToString());
		obj.transform.position = World.Instance.player.transform.position.ToVector2() + dir * (entry.spawnDistance + 1.2f);// 1.2 is circle size?
		obj.GetComponent<FreeNutrient>().timeline = this.timeline;
		obj.GetComponent<FreeNutrient>().timelineEntry = entry;
		if (entry.colorSame)
			obj.GetComponent<FreeNutrient>().soonColor = lastColor;
		else {
			lastColor = obj.GetComponent<FreeNutrient>().soonColor = randomColor();
		}
	}

	public static NutrientColor randomColor() {
		Array values = Enum.GetValues(typeof(NutrientColor));
		int possibleColors = Mathf.Min(values.Length, World.Instance.player.polygon.numsides);
		NutrientColor color = (NutrientColor)values.GetValue(UnityEngine.Random.Range(0, possibleColors));
		Debug.Log(color.ToString());
		
		return color;
	}

	void OnGUI() {
		GUI.Label(new Rect(0, 0, 100, 100), this.timeline.timelimePosition.ToString());
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireSphere(Vector2.zero, Player.PLAYER_RADIUS);
	}
}
