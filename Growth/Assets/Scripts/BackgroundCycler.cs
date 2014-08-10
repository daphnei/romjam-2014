using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundCycler : Pulser {
	const int NUM_LINES = 40;

	public float frequency = 0.1f;
	public float lineDeviance = 20;


	public float minLineColorFraction = 0.6f;
	public float maxLineColorFraction = 0.9f;

	public GameObject bgPolygon;
	
	private List<GameObject> lines;
	private float time = 0;
	private float nextTime = 0;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		this.lines = new List<GameObject>();

		chooseBackgroundColor();

//		float approxDistBetweenLines = Camera.main.pixelWidth / NUM_LINES;
//		for (int i = 0; i < NUM_LINES; i++)
//		{
			GameObject g = GameObject.Instantiate(bgPolygon) as GameObject;
			g.transform.position = new Vector3(0, 0);
			MeshRenderer mr = g.GetComponent<MeshRenderer>() as MeshRenderer;

			mr.material.color = Color.red;
//		}
	}

	static Mesh clone(Mesh mesh)
	{
		Mesh newmesh = new Mesh();
		newmesh.vertices = mesh.vertices;
		newmesh.triangles = mesh.triangles;
		newmesh.uv = mesh.uv;
		newmesh.normals = mesh.normals;
		newmesh.colors = mesh.colors;
		newmesh.tangents = mesh.tangents;
		return newmesh;
	}

		// Update is called once per frame
	void Update () {
		chooseBackgroundColor();

		time = Mathf.MoveTowards(time, nextTime, 0.01f);
	}

	private void chooseBackgroundColor()
	{
		Camera.main.backgroundColor = Camera.main.backgroundColor = new Color(
			Mathf.Sin(frequency*time) * 0.5f + 0.5f,
			Mathf.Sin(frequency*time + 2) * 0.5f + 0.5f,
			Mathf.Sin(frequency*time + 4) * 0.5f + 0.5f
			);
	}
	
	public override void Pulse() {
//		float approxDistBetweenLines = Camera.main.pixelWidth / NUM_LINES;
//		
//		for (int i = 0; i < NUM_LINES; i++)
//		{
//			LineRenderer lr = lines[i].GetComponent<LineRenderer>() as LineRenderer;
//
//			//Choose a new random position
//			float approxXPos = (approxDistBetweenLines * i) + (approxDistBetweenLines/2);
//			updateLinePos(lr, approxXPos);
//			
//			//choose a new random color that is not too far off from the background color.
//			float fract = Random.Range(minLineColorFraction, maxLineColorFraction);
//			lr.material.color = Camera.main.backgroundColor * fract;
//		}
	}
}
