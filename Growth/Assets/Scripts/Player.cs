using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	public CapturedNutrient nutrient;
	private GameObject nutrientParent;
	private List<CapturedNutrient> nutrientList;
	
	public float rotationFriction = 600;
	public float maxRotationSpeed = 1500;
	public float angleExaggerateDistance = 1.75f;
	public float angleExaggerateIncrease = 0.5f;

	public PolygonMaker polygon;
	
	float rotationSpeed;

	Vector3? prevMousePosition;
	float averageRotateSpeed;

	// Use this for initialization
	void Awake () {
		World.Instance.Register(this);

		this.nutrientParent = new GameObject();
		this.nutrientParent.transform.position = this.transform.position;
		this.nutrientList = new List<CapturedNutrient>();
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
			angle *= distance < angleExaggerateDistance ? 1 : 1 + (distance - angleExaggerateDistance) * angleExaggerateIncrease;

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

		this.rotationSpeed = Mathf.Clamp(this.rotationSpeed, -maxRotationSpeed, maxRotationSpeed);
		this.transform.Rotate(new Vector3(0, 0, 1), this.rotationSpeed * Time.deltaTime);
		this.rotationSpeed = this.rotationSpeed.AbsSubtract(rotationFriction * Time.deltaTime);

		if (Input.GetMouseButtonUp(0)) {
			Vector3[] vertices = this.polygon.vertices;
			for (int i = 0; i < vertices.Length; i++) {

			}
		}

		//Slowly rotate the nutrients in the center so that they aren't completely just standing there.
		this.nutrientParent.transform.Rotate(Vector3.forward * Time.deltaTime * 80);
	}

	public void AddNutrient()
	{
		int curNumVertices = this.polygon.vertices.Length;

		//Add a nutrient if we're not at the max.
		if (this.nutrientList.Count < curNumVertices)
		{
			float angleOfNewNut = (360 / curNumVertices) * this.nutrientList.Count;
			float angleOfNewNutInRadians = Mathf.Deg2Rad * angleOfNewNut;
			
			float radius = 0.5f;
			Vector2 targetPosition = new Vector2(
				radius * Mathf.Cos(angleOfNewNutInRadians),
				radius * Mathf.Sin(angleOfNewNutInRadians));

			CapturedNutrient nut = Object.Instantiate(this.nutrient) as CapturedNutrient;
			nut.transform.parent = nutrientParent.transform;

			nut.transform.localPosition = targetPosition;

			this.nutrientList.Add(nut);
		}
		//Reached the max number of nutrients for this polygom. Time to grow an extra side!
		else
		{
			//First delete all the nutrients.
			foreach (CapturedNutrient nut in this.nutrientList)
			{
				Object.Destroy(nut);
			}

			this.nutrientList.Clear();

			this.polygon.addNode();
		}
	}

	public void RemoveNutrient()
	{
	}

	void OnGUI() {
		GUI.Label(new Rect(0, 0, 100, 100), this.rotationSpeed.ToString());
	}
}
