using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyGenerator : MonoBehaviour {

	public class Timeline {

		float timelineStartPosition = 0;
		int timelineIndex = 0;
		List<TimelineEntry> entries = new List<TimelineEntry>();

		public void RestartTimeline() {
			this.timelineStartPosition = Time.timeSinceLevelLoad;
			this.timelineIndex = 0;
		}

		public IEnumerable<TimelineEntry> UpdateTimeline() {
			float timelinePos = Time.timeSinceLevelLoad - this.timelineStartPosition;
			Debug.Log(timelinePos.ToString());
			if (this.timelineIndex >= this.entries.Count) {
				Debug.Log("TOO LATE");
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
		public float spawnTime { get { return hitTime - speed * spawnDistance; } }
	}

	public float minAttachRateInSeconds = 0.5f;
	public float maxAttachRateInSeconds = 3f;

	private float timeUntilSpawn;

	public GameObject[] enemies;
	public Timeline timeline;

	// Use this for initialization
	void Start () {
		this.timeline = Timeline.GenerateTimeline(100, 1f);
		//this.timeUntilSpawn = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilSpawn -= Time.deltaTime;

		foreach (TimelineEntry entry in this.timeline.UpdateTimeline()) {
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
		obj.transform.position = World.Instance.player.transform.position.ToVector2() + Random.onUnitSphere.ToVector2() * (entry.spawnDistance); // TODO: + player-radius
		obj.GetComponent<FreeNutrient>().speed = entry.speed;
	}
}
