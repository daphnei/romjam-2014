using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	const float TAP_TIME = 0.2f;
	const float TAP_RADIUS = 20f;
	const float LINE_WIDTH = 0.1f;

	public float rotationFriction = 600;
	public float maxRotationSpeed = 1500;
	public float angleExaggerateDistance = 1.75f;
	public float angleExaggerateIncrease = 0.5f;

	public PolygonMaker polygon;
	public GameObject bulletContainer;
	public CapturedNutrient nutrientPrefab;

	public float rotationSpeed { get; set; }
	public int numberOfCapturedNutrients { get; set; }

	Vector3? prevMousePosition;
	float touchTime = 0;
	float averageRotateSpeed;
	float prevRealTime;

	private GameObject nutrientParent;
	private List<CapturedNutrient> nutrientList;
	private int lastPlayerRotationDir = 1; //either 1 or -1

	private bool canFire;

	// Use this for initialization
	void Awake() {
		World.Instance.Register(this);

		this.nutrientParent = new GameObject();
		this.nutrientParent.transform.position = this.transform.position;
		this.nutrientList = new List<CapturedNutrient>();
	}

	// Update is called once per frame
	void Update() {
		float realDeltaTime = Time.realtimeSinceStartup - this.prevRealTime;

		if (this.bulletContainer.transform.childCount == 0) {
			this.canFire = true;
			this.bulletContainer.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}

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

//		Time.timeScale = Mathf.Clamp(Mathf.Abs(this.rotationSpeed) / 80f, 0.2f, 3f);

		this.rotationSpeed = Mathf.Clamp(this.rotationSpeed, -maxRotationSpeed, maxRotationSpeed);
		this.transform.Rotate(new Vector3(0, 0, 1), this.rotationSpeed * realDeltaTime);
		this.rotationSpeed = this.rotationSpeed.AbsSubtract(rotationFriction * realDeltaTime);

		this.prevRealTime = Time.realtimeSinceStartup;

		//Slowly rotate the nutrients in the center so that they aren't completely just standing there.
		//Rotate in the opposite direction to how the player is rotating, becuase it looks cool.
		if (this.rotationSpeed != 0)
			this.lastPlayerRotationDir = (this.rotationSpeed > 0 ? -1 : 1);
		this.nutrientParent.transform.Rotate(Vector3.forward * Time.deltaTime * 80 * this.lastPlayerRotationDir);
	}

	private void OnPlayerTappedScreen() {
		if (!this.canFire) {
			return;
		}

		Vector3[] vertices = this.polygon.vertices;
		for (int i = 0; i < vertices.Length; i++) {
			int i1 = i == 0 ? vertices.Length - 1 : i - 1;
			int i2 = i;
			Vector3 v1 = this.polygon.transform.TransformPoint(vertices[i1]);
			Vector3 v2 = this.polygon.transform.TransformPoint(vertices[i2]);
			Vector3 vCenter = (v1 + v2) / 2;

			GameObject g = new GameObject();
			g.transform.position = vCenter;
			g.name = "Bullet";
			g.AddComponent<Bullet>().localVelocity = (vertices[i1] + vertices[i2]);

			LineRenderer lr = g.AddComponent<LineRenderer>();
			lr.transform.parent = this.bulletContainer.transform;
			lr.useWorldSpace = false;
			lr.SetVertexCount(2);
			lr.SetPosition(0, v1 - vCenter);
			lr.SetPosition(1, v2 - vCenter);
			lr.SetWidth(LINE_WIDTH, LINE_WIDTH);

			GameObject colliderObj = new GameObject();
			colliderObj.name = "Collider";
			colliderObj.transform.parent = g.transform;
			colliderObj.transform.position = g.transform.position;
			colliderObj.AddComponent<BulletCollider>().bullet = g.GetComponent<Bullet>();

			Rigidbody2D rb = colliderObj.AddComponent<Rigidbody2D>();
			rb.isKinematic = false;
			rb.gravityScale = 0;

			BoxCollider2D bc = colliderObj.AddComponent<BoxCollider2D>();
			bc.size = new Vector2((v1 - v2).magnitude, LINE_WIDTH);
			bc.transform.Rotate(new Vector3(0, 0, 1), (v1 - v2).ToVector2().AngleFromUnitX());
			bc.isTrigger = true;
		}
	}

	public void AddNutrient() {
		int curNumVertices = this.polygon.vertices.Length;

		//Add a nutrient if we're not at the max.
		if (this.nutrientList.Count < curNumVertices) {
			float angleOfNewNut = (360 / curNumVertices) * this.nutrientList.Count;
			float angleOfNewNutInRadians = Mathf.Deg2Rad * angleOfNewNut;

			float radius = 0.5f;
			Vector2 targetPosition = new Vector2(
				radius * Mathf.Cos(angleOfNewNutInRadians),
				radius * Mathf.Sin(angleOfNewNutInRadians));

			CapturedNutrient nut = Object.Instantiate(this.nutrientPrefab) as CapturedNutrient;
			nut.transform.parent = nutrientParent.transform;
			nut.transform.localPosition = targetPosition;
			nut.transform.localRotation = Quaternion.AngleAxis(0, Vector3.forward);

			this.nutrientList.Add(nut);
		}

		//Reached the max number of nutrients for this polygom. Time to grow an extra side!
		else {

			//First delete all the nutrients.
			foreach (CapturedNutrient nut in this.nutrientList) {
				Object.Destroy(nut.gameObject);
			}

			this.nutrientList.Clear();
			this.polygon.addNode();
		}
	}

	public void RemoveNutrient() {
	    if (this.nutrientList.Count == 0 && this.polygon.numsides > 3) {
			this.polygon.removeNode();

			for (int i = 0; i < this.polygon.numsides; i++) {
				this.AddNutrient();
			}
		} else if (this.nutrientList.Count > 0) {
			CapturedNutrient n = this.nutrientList.Pop();
			GameObject.Destroy(n.gameObject);
		}
	}

	void OnGUI() {
		GUI.Label(new Rect(0, 0, 100, 100), this.rotationSpeed.ToString());
	}
}
