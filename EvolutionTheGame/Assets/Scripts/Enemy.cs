using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	/**
	 * This is a temporary hack until I sync up with the code that has the player.
	 */
	public static Vector2 locationOfPlayer = Vector2.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		DoUpdate();
	}

	virtual public void DoUpdate() {
		//At some point this can be replaced with a check for collision?
		if (Mathf.Abs((this.rigidbody2D.position - locationOfPlayer).magnitude) < 1.5)
		{
			Destroy(this.gameObject);
		}
	}
}
