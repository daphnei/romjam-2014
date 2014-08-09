using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundCycler : Pulser {
	const int NUM_LINES = 40;

	public float frequency = 0.1f;
	public float lineDeviance = 20;

	public float minLineColorFraction = 0.6f;
	public float maxLineColorFraction = 0.9f;

	public BackgroundPolygon bgPolygon;
	
	private List<GameObject> lines;
	
	// Use this for initialization
	void Start () {
		base.Start();
		
		this.lines = new List<GameObject>();

		chooseBackgroundColor();

		//Create a bunch of random vertical-ish lines.
		float approxDistBetweenLines = Camera.main.pixelWidth / NUM_LINES;
//		for (int i = 0; i < NUM_LINES; i++)
//		{
			GameObject g = Instantiate(bgPolygon) as GameObject;
			g.transform.position = new Vector2(0, 0);
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
	}

	private void chooseBackgroundColor()
	{
		Camera.main.backgroundColor = Camera.main.backgroundColor = new Color(
			Mathf.Sin(frequency*Time.timeSinceLevelLoad) * 0.5f + 0.5f,
			Mathf.Sin(frequency*Time.timeSinceLevelLoad + 2) * 0.5f + 0.5f,
			Mathf.Sin(frequency*Time.timeSinceLevelLoad + 4) * 0.5f + 0.5f
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
