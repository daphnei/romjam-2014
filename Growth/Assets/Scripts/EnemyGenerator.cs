using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour {
	public float minAttachRateInSeconds = 0.5f;
	public float maxAttachRateInSeconds = 3f;

	private float timeUntilSpawn;

	public GameObject[] enemies;

	// Use this for initialization
	void Start () {

		timeUntilSpawn = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilSpawn -= Time.deltaTime;

		if (timeUntilSpawn <= 0)
		{
			timeUntilSpawn = Random.Range(minAttachRateInSeconds, maxAttachRateInSeconds);

			SpawnEnemy();
		}
	}

	void SpawnEnemy()
	{
		if (Camera.main == null)
		{
			throw new UnityException("This should not happen YOLOSWAG");
		}

		int enemyIndex = Random.Range(0, enemies.Length);
		Instantiate(enemies[enemyIndex]);
	}
}
