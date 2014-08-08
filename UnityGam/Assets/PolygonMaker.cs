using UnityEngine;
using System.Collections;

public class PolygonMaker : MonoBehaviour {
	
	private MeshFilter filter;

	public int _numsides = 3; //public to set in editor. pls no abus.
	public int numsides {
		get { return this._numsides; }
		set 
		{
			if (value >= 3 && value !=this._numsides) {
				this._numsides = value;
				this.filter.mesh = makeMesh(this._numsides);
			}
		}
	}

	private bool transitioning = false;
	private bool growing = true;
	private float transElapsed = 0f;
	static float transtime = 1f;

	public static Mesh makeMesh(int numsides) {
		Mesh m = new Mesh ();
		Vector3[] verts = new Vector3[numsides];
		Vector2[] uv = new Vector2[numsides];
		int[] tris = new int[numsides * 3];
		
		for (int i = 0; i < numsides; i++) {
			verts[i] = Quaternion.AngleAxis(360f / numsides * -i, Vector3.forward) * Vector3.up;

			uv[i] = new Vector2(0, 0); //TODO are we even going to be texturing

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
			updateTransitionalMesh(0f, m);
			filter.mesh = m;
		}
	}

	// n in (0,1)
	float easing(float n) {
		return Mathf.Sin(n*(Mathf.PI/2)); //TODO this
	}

	// Use this for initialization
	void Start() {
		filter = this.gameObject.GetComponent<MeshFilter>();
		this.filter.mesh = makeMesh(this._numsides);
	}

	void updateTransitionalMesh(float firstangle, Mesh m) {
		int ns = m.vertices.Length;
		Vector3[] verts = new Vector3[ns];
		verts[0] = Quaternion.AngleAxis(0, Vector3.forward) * Vector3.up;
		for (int i = 1; i < ns; i++) {
			verts[i] = Quaternion.AngleAxis(-firstangle - ((360f - firstangle) / (ns-1)) * (i-1), Vector3.forward) * Vector3.up;
		}
		m.vertices = verts;
	}

	// Update is called once per frame
	void Update () {
		transElapsed += Time.deltaTime;

		if (!transitioning) {
			if (Input.GetKey(KeyCode.A)) {
				addNode();
			}
			if (Input.GetKey(KeyCode.S)) {
				removeNode();
			}
		}
		else if (transElapsed < transtime){
			float angsrc, angdest;
			if (growing) {
				angsrc = 0;
				angdest = 360 / (this._numsides + 1);
			}else {
				angsrc = 360 / this._numsides;
				angdest = 0;
			}
			
			float firstangle = angsrc + (angdest - angsrc) * easing(transElapsed / transtime);
			updateTransitionalMesh(firstangle, filter.mesh);
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
