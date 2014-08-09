using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	const float TAP_TIME = 0.2f;
	const float TAP_RADIUS = 20f;

	public float rotationFriction = 600;
	public float maxRotationSpeed = 1500;
	public float angleExaggerateDistance = 1.75f;
	public float angleExaggerateIncrease = 0.5f;

	public PolygonMaker polygon;
	public GameObject bulletContainer;

	public int NumCapturedNutrients = 0;

	float rotationSpeed;

	Vector3? prevMousePosition;
	float touchTime = 0;
	float averageRotateSpeed;
	float prevRealTime;

	// Use this for initialization
	void Awake () {
		World.Instance.Register(this);

	}
	
	// Update is called once per frame
	void Update () {
		float realDeltaTime = Time.realtimeSinceStartup - this.prevRealTime;

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
			angle *= distance < angleExaggerateDistance ? 1 : 1 + (distance - angleExaggerateDistance) * angleExaggerateIncrease;

			this.rotationSpeed = angle / realDeltaTime;
			this.averageRotateSpeed = this.averageRotateSpeed == 0 ? this.rotationSpeed : (this.rotationSpeed + this.averageRotateSpeed) / 2;
			this.prevMousePosition = Input.mousePosition;
			this.touchTime += realDeltaTime;
		} else {
			if (this.prevMousePosition.HasValue) {
				if (this.touchTime < TAP_TIME) {
					if ((this.prevMousePosition.Value - Input.mousePosition).magnitude < TAP_RADIUS) {
						this.OnPlayerTappedScreen();
					}
				}

				this.rotationSpeed = this.averageRotateSpeed;
				this.prevMousePosition = null;
				this.averageRotateSpeed = 0;
				this.touchTime = 0;
			}
		}
		Time.timeScale = Mathf.Clamp(Mathf.Abs(this.rotationSpeed) / 80f, 0.2f, 3f);
		this.rotationSpeed = Mathf.Clamp(this.rotationSpeed, -maxRotationSpeed, maxRotationSpeed);
		this.transform.Rotate(new Vector3(0, 0, 1), this.rotationSpeed * realDeltaTime);
		this.rotationSpeed = this.rotationSpeed.AbsSubtract(rotationFriction * realDeltaTime);

		this.prevRealTime = Time.realtimeSinceStartup;
	}

	private void OnPlayerTappedScreen() {
		Vector3[] vertices = this.polygon.vertices;
		for (int i = 0; i < vertices.Length; i++) {
			int i1 = i == 0 ? vertices.Length - 1 : i - 1;
			int i2 = i;
			Vector3 v1 = this.polygon.transform.TransformPoint(vertices[i1]);
			Vector3 v2 = this.polygon.transform.TransformPoint(vertices[i2]);
			GameObject g = new GameObject();
			g.AddComponent<Bullet>().localVelocity = (vertices[i1] + vertices[i2]) / 2;
			LineRenderer lr = g.AddComponent<LineRenderer>();
			lr.transform.parent = this.bulletContainer.transform;
			lr.useWorldSpace = false;
			lr.SetVertexCount(2);
			lr.SetPosition(0, v1);
			lr.SetPosition(1, v2);
			lr.SetWidth(0.1f, 0.1f);
		}
	}

	void OnGUI() {
		GUI.Label(new Rect(0, 0, 100, 100), this.rotationSpeed.ToString());
	}
}
