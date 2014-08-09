using UnityEngine;
using System.Collections;

public class PolygonMaker : MonoBehaviour {

	private PolygonCollider2D pcollider;

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
				this.pcollider.CreatePrimitive(this._numsides, new Vector2(1, 1), new Vector2(0, 0));
			}
		}
	}

	private bool transitioning = false;
	private bool growing = true;
	private float transElapsed = 0f;
	static float transtime = 0.5f;

	public static Mesh makeMesh(int numsides) {
		Mesh m = new Mesh ();
		Vector3[] verts = new Vector3[numsides];
		Vector2[] uv = new Vector2[numsides];
		int[] tris = new int[numsides * 3];
		
		for (int i = 0; i < numsides; i++) {
			verts[i] = Quaternion.AngleAxis(360f / numsides * -i, Vector3.forward) * Vector3.up;

			uv[i] = new Vector2(0.5f+verts[i].x/2, 0.5f+verts[i].y/2);

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
			transitioning = true;
			transElapsed = 0f;
		}
	}

	public void addNode() {
		if (!transitioning) {
			growing = true;
			transitioning = true;
			transElapsed = 0f;

			Mesh m = makeMesh(numsides + 1);
			updateTransitionalMesh(0f, 0f, m);
			filter.mesh = m;
		}
	}

	// Use this for initialization
	void Start() {
		filter = this.gameObject.GetComponent<MeshFilter>();
		pcollider = this.gameObject.GetComponent<PolygonCollider2D>();
		this.filter.mesh = makeMesh(this._numsides);
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
			uv[i] = new Vector2(0.5f + verts[i].x / 2, 0.5f + verts[i].y / 2);
		}
		
		m.vertices = verts;
		m.uv = uv;
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
	}
}
