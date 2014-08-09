using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	// Use this for initialization
	void Start () {
		DoStart();
	}
	
	// Update is called once per frame
	void Update () {
		DoUpdate();
	}

	virtual public void DoStart()
	{
		ChooseSpawnPoint();
	}

	virtual public void DoUpdate() {
		Vector3 positionOfPlayer = World.Instance.player.transform.position;

		//At some point this can be replaced with a check for collision?
		if (Mathf.Abs((this.transform.position - positionOfPlayer).magnitude) < 1.5)
		{
			Destroy(this.gameObject);
		}
	}

	//Spawn on any of the edges of the screen.
	virtual protected void ChooseSpawnPoint()
	{
		//How far off screen to do initial position;
		int d = 3;

		//Choose a spawn point on one of the edges of the viewport.
		Vector2 spawnPoint;
		if (Random.value >= 0.5)
		{ 
			//spawn with a random x
			spawnPoint = new Vector2(
				Random.Range(0, Camera.main.pixelWidth),
				Random.value >= 0.5 ? -d : Camera.main.pixelHeight + d);
			spawnPoint = Camera.main.ScreenToWorldPoint(spawnPoint);
		}
		else
		{
			//spawn with a random y
			spawnPoint = new Vector2(
				Random.value >= 0.5 ? -d : Camera.main.pixelWidth,
				Random.Range(0, Camera.main.pixelHeight + d));
			spawnPoint = Camera.main.ScreenToWorldPoint(spawnPoint);
		}

		this.transform.position = spawnPoint;
	}
}
