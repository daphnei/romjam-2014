using UnityEngine;
using System.Collections;
using System;

public class PolygonMaker : MonoBehaviour {

	private CircleCollider2D ccollider;

	//I know this hsouldn't be public with a getter setter, but I cannot too lazy to figure out the syntax.
	public MeshFilter filter;
	public Vector3[] vertices { get { return this.filter.mesh.vertices; } }

	public int _numsides = 3; //public to set in editor. pls no abus.
	public int numsides {
		get { return this._numsides; }
		set 
		{
			if (value >= 3 && value !=this._numsides) {
				this._numsides = value;
				this.filter.mesh = makeMesh(this._numsides);
				this.ccollider.radius = (float) Math.Cos(Mathf.Deg2Rad * (360 / numsides /2));
				if (this.NumberOfSidesChanged != null) {
					this.NumberOfSidesChanged();
				}
			}
		}
	}

	private float spinCount = 0f;
	static float spinTime = 1f;
	static float numRotations = 8f;

	public event Action NumberOfSidesChanged;
	public event Action NumberOfSidesTransitionStart;

	private bool transitioning = false;
	private bool growing = true;
	private float transElapsed = 0f;
	static float transtime = 0.5f;

	// Use this for initialization
	void Awake() {
		filter = this.gameObject.GetComponent<MeshFilter>();
		ccollider = this.gameObject.GetComponent<CircleCollider2D>();
		this.numsides = 4;
		ImageManager.loadMaterials();
		this.renderer.materials = ImageManager.updateTexture(_numsides-3);
		this.ccollider.radius = (float)Math.Cos(Mathf.Deg2Rad * (360 / numsides));
	}


	public static Mesh makeMesh(int numsides) {
		Mesh m = new Mesh ();
		Vector3[] verts = new Vector3[numsides];
		Vector2[] uv = new Vector2[numsides];
		int[] tris = new int[numsides * 3];
		
		for (int i = 0; i < numsides; i++) {
			verts[i] = Quaternion.AngleAxis(360f / numsides * -i, Vector3.forward) * Vector3.up;

			uv[i] = v2uv(verts[i]);

			tris[3 * i] = 0;
			tris[3 * i + 1] = i;
			tris[3 * i + 2] = (i + 1) % numsides;

			//Debug.Log(verts[i]);
		}
		
		m.vertices = verts;
		m.triangles = tris;
		m.uv = uv;

		m.RecalculateNormals();
		return m;
	}

	public void removeNode() {
		if (!transitioning && numsides>3) {
			growing = false;
			this.renderer.materials = ImageManager.updateTexture(_numsides - 4); growing = false;
			transitioning = true;
			transElapsed = 0f;
			if (this.NumberOfSidesTransitionStart != null) {
				this.NumberOfSidesTransitionStart();
			}
			Spin();
		}
	}

	public void addNode() {
		if (!transitioning) {
			this.renderer.materials = ImageManager.updateTexture(_numsides - 2);
			growing = true;
			transitioning = true;
			transElapsed = 0f;

			Mesh m = makeMesh(numsides + 1);
			updateTransitionalMesh(0f, 0f, m);
			filter.mesh = m;
			if (this.NumberOfSidesTransitionStart != null) {
				this.NumberOfSidesTransitionStart();
			}
			Spin();
		}
	}

	public void Spin() {
		this.spinCount = 0f;
	}

	void updateTransitionalMesh(float firstAngle, float offsetAngle, Mesh m) {
		int ns = m.vertices.Length;
		Vector3[] verts = new Vector3[ns];
		Vector2[] uv = new Vector2[ns];
		verts[0] = Quaternion.AngleAxis(offsetAngle, Vector3.forward) * Vector3.up;
		float angstep = ((360f - firstAngle) / (ns - 1));
		for (int i = 1; i < ns; i++) {
			verts[i] = Quaternion.AngleAxis(-firstAngle + offsetAngle - angstep * (i - 1), Vector3.forward) * Vector3.up;
		}
		for (int i = 0; i < ns; i++) {
			uv[i] = v2uv(verts[i]);
		}
		
		m.vertices = verts;
		m.uv = uv;
	}

	static Vector2 v2uv(Vector3 v) {
		return new Vector2(0.5f + v.x / 2, 0.5f + v.y / 2);
	}

	void updateTextureUVs(float angleOffset) {
		Mesh m = this.filter.mesh;
		Vector3[] verts = m.vertices;
		Vector2[] uv = new Vector2[verts.Length];
		for (int i = 0; i < verts.Length; i++) {
			uv[i] = v2uv(
				Quaternion.AngleAxis(
				(this.transform.eulerAngles.z - angleOffset),
				Vector3.forward) * new Vector3(verts[i].x, verts[i].y, 0));
		}
		m.uv = uv;
		this.filter.mesh = m;
	}

	// Update is called once per frame
	void Update () {
		transElapsed += Time.deltaTime;

		if (!transitioning) {
			if (Input.GetKey(KeyCode.S)) {
				addNode();
			}
			if (Input.GetKey(KeyCode.A)) {
				removeNode();
			}
		}
		else if (transElapsed < transtime){
			float angsrc, angdest, ofsdest;
			if (growing) {
				angsrc = 0;
				angdest = 360 / (this._numsides + 1);
				ofsdest = -2 * 360 / (this._numsides + 1);
			}else {
				angsrc = 360 / this._numsides;
				angdest = 0;
				ofsdest = 360 / (this._numsides-1);
			}
			float ease = Easing.easeSin(transElapsed / transtime);
			float firstangle = angsrc + (angdest - angsrc) * ease;
			updateTransitionalMesh(firstangle, ofsdest * ease, filter.mesh);
		}
		else {
			transitioning = false;
			if (growing) {
				numsides = numsides + 1;
			}
			else {
				numsides = numsides - 1;
			}
			//Debug.Log(numsides);
		}


		if (spinCount < spinTime) {
			updateTextureUVs(360 * numRotations * Easing.easeSinInv(spinCount / spinTime));
			spinCount += Time.deltaTime;
		}
		else {
			updateTextureUVs(0);
		}
	}
}
