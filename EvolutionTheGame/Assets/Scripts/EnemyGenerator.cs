using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour {
	public float minAttachRateInSeconds = 0.5f;
	public float maxAttachRateInSeconds = 4f;

	private float timeUntilSpawn;

	public GameObject[] enemies;

	// Use this for initialization
	void Start () {

		timeUntilSpawn = Random.Range(minAttachRateInSeconds, maxAttachRateInSeconds);
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

		//Choose a spawn point on one of the edges of the viewport.
		Vector2 spawnPoint;
		if (Random.value >= 0.5)
		{ 
			//spawn with a random x
			spawnPoint = new Vector2(Random.Range(0, Camera.main.pixelWidth), Random.value >= 0.5 ? -1 : Camera.main.pixelHeight+1);
			spawnPoint = Camera.main.ScreenToWorldPoint(spawnPoint);
		}
		else
		{
			//spawn with a random y
			spawnPoint = new Vector2(Random.value >= 0.5 ? 0 : Camera.main.pixelWidth, Random.Range(-1, Camera.main.pixelHeight+1));
			spawnPoint = Camera.main.ScreenToWorldPoint(spawnPoint);
		}

		int enemyIndex = Random.Range(0, enemies.Length);
		GameObject newEnemy = Instantiate(enemies[enemyIndex]) as GameObject;
		newEnemy.transform.position = spawnPoint;
	}
}
