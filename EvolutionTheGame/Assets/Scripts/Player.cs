using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	Vector3? prevMousePosition;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			if (!this.prevMousePosition.HasValue) {
				this.prevMousePosition = Input.mousePosition;
			}
			Vector3 worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 worldMousePrev = Camera.main.ScreenToWorldPoint(this.prevMousePosition.Value);
			this.transform.position += worldMouse - worldMousePrev;
			this.prevMousePosition = Input.mousePosition;
		} else {
			this.prevMousePosition = null;
		}
	}
}
