using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		DoUpdate();
	}

	virtual public void DoUpdate() {
		Vector3 positionOfPlayer = World.GetInstance().player.transform.position;

		//At some point this can be replaced with a check for collision?
		if (Mathf.Abs((this.transform.position - positionOfPlayer).magnitude) < 1.5)
		{
			Destroy(this.gameObject);
		}
	}
}
