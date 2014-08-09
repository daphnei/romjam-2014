using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float RotationFriction = 600;
	public float MaxRotationSpeed = 1500;
	public float AngleExaggerateDistance = 1.75f;
	public float AngleExaggerateIncrease = 0.5f;

	float rotationSpeed;

	Vector3? prevMousePosition;
	float averageRotateSpeed;

	// Use this for initialization
	void Awake () {
		World.Instance.Register(this);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			if (!this.prevMousePosition.HasValue) {
				this.prevMousePosition = Input.mousePosition;
			}

			Vector3 worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition).Flatten();
			Vector3 worldMousePrev = Camera.main.ScreenToWorldPoint(this.prevMousePosition.Value).Flatten();
			Vector3 prevAngleV = worldMouse - this.transform.position;
			Vector3 angleV = worldMousePrev - this.transform.position;
			float angle = Vector3.Angle(prevAngleV, angleV) * Mathf.Sign(Vector3.Dot(prevAngleV, angleV.ToVector2().Rotate90DegreesCounterClockwise()));
			float distance = (worldMouse - this.transform.position).magnitude;
			angle *= distance < AngleExaggerateDistance ? 1 : 1 + (distance - AngleExaggerateDistance) * AngleExaggerateIncrease;

			this.rotationSpeed = angle / Time.deltaTime;
			this.averageRotateSpeed = this.averageRotateSpeed == 0 ? this.rotationSpeed : (this.rotationSpeed + this.averageRotateSpeed) / 2;
			this.prevMousePosition = Input.mousePosition;
		} else {
			if (this.prevMousePosition.HasValue) {
				this.rotationSpeed = this.averageRotateSpeed;
				this.prevMousePosition = null;
				this.averageRotateSpeed = 0;
			}
		}

		this.rotationSpeed = Mathf.Clamp(this.rotationSpeed, -MaxRotationSpeed, MaxRotationSpeed);
		this.transform.Rotate(new Vector3(0, 0, 1), this.rotationSpeed * Time.deltaTime);
		this.rotationSpeed = this.rotationSpeed.AbsSubtract(RotationFriction * Time.deltaTime);
	}

	void OnGUI() {
		GUI.Label(new Rect(0, 0, 100, 100), this.rotationSpeed.ToString());
	}
}
