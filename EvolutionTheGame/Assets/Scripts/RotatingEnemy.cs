using UnityEngine;
using System.Collections;

public class RotatingEnemy : Enemy {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		DoUpdate();
	}

	override public void DoUpdate()
	{
		base.DoUpdate();
		
		this.transform.RotateAround(locationOfPlayer, Vector3.forward, 2);
		
		Vector3 directionToPlayer = -(this.rigidbody2D.position - locationOfPlayer);
		this.transform.position += (directionToPlayer / 200);
	}
}
