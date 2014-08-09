using UnityEngine;
using System.Collections;

/**
 * A critter is anything that spawns outside of the scene
 * and will eventually come toward the player.
 * 
 * IE: Enemies, Nutrients
 **/
public class Critter : Pulser {

	Vector3 initialScale;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		DoStart();
		this.initialScale = this.transform.localScale;
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
		if (Mathf.Abs((this.transform.position - positionOfPlayer).magnitude) < 1.5f)
		{
			HitThePlayer();
		}

		if (this.transform.localScale != Vector3.one) {
			this.transform.localScale = Vector3.MoveTowards(this.transform.localScale, this.initialScale, 0.02f);
//			Debug.Log (this.transform.localScale);
		}
	}

	virtual protected void HitThePlayer()
	{
		Destroy(this.gameObject);
	}                            

	//Spawn on any of the edges of the screen.
	virtual protected void ChooseSpawnPoint()
	{
		//How far off screen to do initial position;
		int d = 3;

		//Choose a spawn point on one of the edges of the viewport.
		Vector2 spawnPoint;
		if (Random.value >= 0.5f)
		{ 
			//spawn with a random x
			spawnPoint = new Vector2(
				Random.Range(0, Camera.main.pixelWidth),
				Random.value >= 0.5f ? -d : Camera.main.pixelHeight + d);
			spawnPoint = Camera.main.ScreenToWorldPoint(spawnPoint);
		}
		else
		{
			//spawn with a random y
			spawnPoint = new Vector2(
				Random.value >= 0.5f ? -d : Camera.main.pixelWidth,
				Random.Range(0, Camera.main.pixelHeight + d));
			spawnPoint = Camera.main.ScreenToWorldPoint(spawnPoint);
		}

		this.transform.position = spawnPoint;
	}

	public override void Pulse ()
	{
		base.Pulse();
		this.transform.localScale = this.initialScale * 2;
	}
}
