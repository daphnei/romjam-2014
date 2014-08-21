using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

	public const float LINE_WIDTH = 0.17f;
	public const float PLAYER_RADIUS = 1.3f;
	const float TAP_TIME = 0.2f;
	const float TAP_RADIUS = 20f;
	const float SPAWN_TIME = 2f;

	public float rotationFriction = 600;
	public float maxRotationSpeed = 1500;
	public float angleExaggerateDistance = 1.75f;
	public float angleExaggerateIncrease = 0.5f;

	public PolygonMaker polygon;
	public GameObject bulletContainer;
	public CapturedNutrient nutrientPrefab;

	public List<NutrientColor> SideColors;

	public float rotationSpeed { get; set; }

	public Material bulletMaterial;

	Vector3? prevMousePosition;
	float touchTime = 0;
	float averageRotateSpeed;
	float prevRealTime;
	float spawnTimer = 0;

	private List<NutrientColor> capturedNutrientList;
	private int lastPlayerRotationDir = 1; //either 1 or -1

	private bool canFire = true;
	private bool pulseOut = false;

	/**
	 * Showa take damage animation for this many frames;
	 */
	private int takeDamage = 0;
	public int numFramesToDoDamageVibrateFor = 15;

	public Player() : base(){
	}

	// Use this for initialization
	void Awake() {
		World.Instance.Register(this);

		this.capturedNutrientList = new List<NutrientColor>();

		this.polygon.NumberOfSidesChanged += this.CreateBulletRing;
		this.polygon.NumberOfSidesTransitionStart += this.DeleteBullets;
	}

	void Start() {
		this.DeleteBullets();
		this.CreateBulletRing();
	}

	// Update is called once per frame
	void Update() {
		if (this.pulseOut) {
			this.transform.localScale = Vector3.MoveTowards(this.transform.localScale, new Vector3(1.3f, 1.3f, 1.3f), 0.1f);
			if (this.transform.localScale == new Vector3(1.3f, 1.3f, 1.3f)) {
				this.pulseOut = false;
			}
		} else if (this.transform.localScale != Vector3.one) {
			this.transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, 0.02f);
		}

		float realDeltaTime = Time.realtimeSinceStartup - this.prevRealTime;

		/*if (this.bulletContainer.transform.childCount == 0) {
			this.canFire = true;
			this.bulletContainer.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}*/

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
						// this.CreateBulletRing();
					}
				}

				this.rotationSpeed = this.averageRotateSpeed;
				this.prevMousePosition = null;
				this.averageRotateSpeed = 0;
				this.touchTime = 0;
			}
		}

		if (this.takeDamage > 0)
		{
			Vector2 newPosition;
			if (this.takeDamage == 1)
			{
				newPosition = Vector2.zero;
			}
			else
			{
				newPosition = new Vector2(UnityEngine.Random.Range(-.1f, .1f), UnityEngine.Random.Range(-.1f, .1f));
			}

			this.transform.position = newPosition;
			this.takeDamage--;
		}

		//		Time.timeScale = Mathf.Clamp(Mathf.Abs(this.rotationSpeed) / 80f, 0.2f, 3f);

		this.rotationSpeed = Mathf.Clamp(this.rotationSpeed, -maxRotationSpeed, maxRotationSpeed);
		this.transform.Rotate(new Vector3(0, 0, 1), this.rotationSpeed * realDeltaTime);
		this.rotationSpeed = this.rotationSpeed.AbsSubtract(rotationFriction * realDeltaTime);

		this.prevRealTime = Time.realtimeSinceStartup;

		// Slowly rotate the nutrients in the center so that they aren't completely just standing there.
		// Rotate in the opposite direction to how the player is rotating, becuase it looks cool.
		if (this.rotationSpeed != 0)
			this.lastPlayerRotationDir = (this.rotationSpeed > 0 ? -1 : 1);

		// Handle spawn timers.
		/*this.spawnTimer -= Time.deltaTime;
		if (this.spawnTimer <= 0) {
			this.CreateBulletRing();
			this.spawnTimer = SPAWN_TIME;
		}*/
	}

	void DeleteBullets() {
		foreach (Transform t in this.bulletContainer.transform) {
			Destroy(t.gameObject);
		}
	}

	private void CreateBulletRing() {
		if (!this.canFire) {
			return;
		}

		Vector3[] vertices = this.polygon.vertices;
		Array colorVals = Enum.GetValues(typeof(NutrientColor));
		this.SideColors.Clear();
		for (int i = 0; i < vertices.Length; i++) {
			this.SideColors.Add((NutrientColor)colorVals.GetValue(i % colorVals.Length));
		}

		//int missingIndex = Random.Range(0, vertices.Length - 1);
		for (int i = 0; i < vertices.Length; i++) {
			int i1 = i == 0 ? vertices.Length - 1 : i - 1;
			int i2 = i;
			this.CreateBullet(vertices, i1, i2, this.SideColors[i]);
		}
	}

	private Bullet CreateBullet(Vector3[] vertices, int i1, int i2, NutrientColor color) {
		Vector3 v1 = this.polygon.transform.TransformPoint(vertices[i1]);
		Vector3 v2 = this.polygon.transform.TransformPoint(vertices[i2]);
		Vector3 vCenter = (v1 + v2) / 2;

		GameObject g = new GameObject();
		g.transform.position = vCenter * 1.2f;
		g.name = "Bullet";

		Bullet bullet = g.AddComponent<Bullet>();
		//bullet.localVelocity = (vertices[i1] + vertices[i2]) / 2;
		bullet.lengthPerDistance = (v1 - v2).magnitude / (vCenter - this.transform.position).magnitude;
		bullet.centerPosition = this.transform.position;

		LineRenderer lr = g.AddComponent<LineRenderer>();
		lr.transform.parent = this.bulletContainer.transform;
		lr.SetVertexCount(2);
		/* lr.useWorldSpace = false;
		lr.SetPosition(0, v1 - vCenter);
		lr.SetPosition(1, v2 - vCenter); */
		lr.material = bulletMaterial;
		lr.SetWidth(LINE_WIDTH, LINE_WIDTH);
		lr.castShadows = false;
		lr.receiveShadows = false;

		bullet.lineRenderer = lr;
		bullet.Color = color;

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
		bullet.bulletCollider = bc;

		return bullet;
	}

	public int CurVerts() {
		return this.polygon.numsides;
	}

	/**
	 * Takes in the color of the captured nutrient.
	 */
	public void AddNutrient(NutrientColor color, bool increaseScore=true, bool updateUI=true) {
		int curNumVertices = this.polygon.vertices.Length;

		//Add a nutrient if we're not at the max.
		if (this.capturedNutrientList.Count < curNumVertices) {
			this.capturedNutrientList.Add (color);

			if (updateUI)
			{
				updateGradientOnPolygonMesh();
				
				this.particleSystem.startColor = color.ColorValue();
				this.particleSystem.Emit(20);
			}
		}

		//Reached the max number of nutrients for this polygom. Time to grow an extra side!
		else {
			this.capturedNutrientList.Clear();
			PulseController.Instance.ChangeNumLayers(this.polygon.numsides + 1 - 3);
			this.polygon.addNode();

			//Update the background to show the new polygon.
			World.Instance.background.UpdateMeshWithNewVertexCount(this.polygon.vertices.Count());

			World.Instance.ClearScreen();
		}

		if (increaseScore) {
			World.Instance.score.Increment(this.polygon.vertices.Count());
		}

		this.pulseOut = true;
	}

	public void RemoveNutrient() {
		if (this.capturedNutrientList.Count == 0 && this.polygon.numsides > 3) {
			PulseController.Instance.ChangeNumLayers(this.polygon.numsides - 1 - 3);
			this.polygon.removeNode();

			for (int i = 0; i < this.polygon.numsides; i++) {
				this.AddNutrient(EnemyGenerator.randomColor(), false, false);
			}

			this.updateGradientOnPolygonMesh();

		} else if (this.capturedNutrientList.Count > 0) {
			this.capturedNutrientList.Pop();
			
			this.takeDamage = numFramesToDoDamageVibrateFor;

			this.updateGradientOnPolygonMesh();
		}

		//Decrease the score. This will also reset the score multiplier.
		World.Instance.score.Decrement(this.polygon.vertices.Count());
	}

	public void ResetNutrients() {
		while (this.capturedNutrientList.Count > 0) {
			RemoveNutrient();
		}
	}

	public void updateGradientOnPolygonMesh()
	{
		Mesh mesh = this.polygon.filter.mesh;

		Color[]  colorsForMesh = new Color[mesh.uv.Length];

		// Instead if vertex.y we use uv.x
		for (int i = 0; i < mesh.uv.Length;i++)
		{
			colorsForMesh[i] = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
			if (i < this.capturedNutrientList.Count)
			{
				colorsForMesh[i] = Extensions.ColorValue(this.capturedNutrientList[i]);
			}
			else
			{
				colorsForMesh[i] = new Color(1, 1, 1, 1);
			}
		}
		
		mesh.colors = colorsForMesh;
	}
}
