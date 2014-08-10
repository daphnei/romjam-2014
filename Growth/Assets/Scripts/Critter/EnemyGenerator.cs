using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
		for (int i = 0; i < entries.Length; i++) {
			if (Random.Range(0, 9) == 0) {
				continue;
			}
			TimelineEntry entry = entries[i] = new TimelineEntry();
			entry.hitTime = i * timeStep;
			entry.speed = 0.5f + i * 0.1f;
			entry.spawnDistance = 6f;
		}
		timeline.entries = entries.Where(e => e != null).OrderBy<TimelineEntry, float>(e => e.spawnTime).ToList<TimelineEntry>();
		return timeline;
	}
}

public class TimelineEntry {
	public float hitTime;
	public float speed;
	public float spawnDistance;
	public float spawnTime { get { return hitTime - spawnDistance / speed; } }

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

	public float timestep = 1f;
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

	void SpawnEnemy(TimelineEntry entry)
	{
		if (Camera.main == null)
		{
			throw new UnityException("This should not happen YOLOSWAG");
		}

		int enemyIndex = Random.Range(0, enemies.Length);
		GameObject obj = Instantiate(enemies[enemyIndex]) as GameObject;
		Vector2 dir = Random.insideUnitCircle.normalized;
		Debug.Log(dir.magnitude.ToString());
		obj.transform.position = World.Instance.player.transform.position.ToVector2() + dir * (entry.spawnDistance + 1.2f);// 1.2 is circle size?
		obj.GetComponent<FreeNutrient>().timeline = this.timeline;
		obj.GetComponent<FreeNutrient>().timelineEntry = entry;
	}

	void OnGUI() {
		GUI.Label(new Rect(0, 0, 100, 100), this.timeline.timelimePosition.ToString());
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireSphere(Vector2.zero, Player.PLAYER_RADIUS);
	}
}
