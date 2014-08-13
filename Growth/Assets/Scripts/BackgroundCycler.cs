using UnityEngine;
using System.Collections;

public class BackgroundCycler : Pulser {
	
	public float frequency = 0.1f;
	public float cycleMovementSpeed = 2f;
	
	public int redMax = 255;
	public int greenMax = 255;
	public int blueMax = 255;
	
	private float time;
	private float nextTime;

	private const int NUM_POLYGONS = 13;
	public GameObject bgPolygon;
	private BackgroundPolygon[] polygons;
	
	protected override void  Start()
	{
		World.Instance.Register(this);

		Player player = World.Instance.player;

		PulseController.Instance.AddPulser(this);
		time = Time.timeSinceLevelLoad;
		nextTime = Time.timeSinceLevelLoad;

		Color c1 = getColor();
		Color c2 = c1 * .7f;

		//Rotate all polys in the background to the player's current rotation if there is a player.
		//Otherwise use a random angle.
		Quaternion q = (player != null) ? player.transform.rotation : 
										  Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

		polygons = new BackgroundPolygon[NUM_POLYGONS];
		GameObject bgPolygonsoContainer = new GameObject("Background Container");

		//Make sure this is at the very back
		bgPolygonsoContainer.transform.position = new Vector3(0,0,10);
	
		for (int i = 0; i < NUM_POLYGONS; i++)
		{
			GameObject g = GameObject.Instantiate(bgPolygon) as GameObject;
			g.transform.parent = bgPolygonsoContainer.transform;

			MeshRenderer mr = g.GetComponent<MeshRenderer>() as MeshRenderer;
			BackgroundPolygon bgPoly = g.GetComponent<BackgroundPolygon>() as BackgroundPolygon;

			polygons[i] = bgPoly;

			mr.sortingLayerName = "bkg";
			mr.sortingOrder = 0;

			refreshBackground(i, c1, c2, q);
		}

		Camera.main.backgroundColor = c2;

		//Player will be null if we are in the menus.
		if (player != null)
		{
			UpdateMeshWithNewVertexCount(player.polygon.vertices.Length);
		}
		else
		{
			UpdateMeshWithNewVertexCount(7);
		}
	}

	private void refreshBackground(int index, Color c1, Color c2, Quaternion desiredRotation)
	{
		GameObject g = polygons[index].gameObject;

		float randomScale = Random.Range(1.4f, 1.6f);

		g.transform.position = new Vector3(0, 0, index + 10);
		g.transform.localScale = new Vector2(randomScale * index, randomScale * index);

		g.transform.rotation = desiredRotation;

		MeshRenderer mr = polygons[index].gameObject.GetComponent<MeshRenderer>() as MeshRenderer;

		mr.material.color = (index%2 == 0) ? c1 : c2;
	}

	public void UpdateMeshWithNewVertexCount(int numVertices)
	{
		Mesh newMesh = PolygonMaker.makeMesh(numVertices);
		foreach (BackgroundPolygon bgPoly in this.polygons)
		{
			bgPoly.SetMesh(newMesh);
		}
	}

	public override void Pulse ()
	{
		base.Pulse();
		
		nextTime = Time.timeSinceLevelLoad;

		Color c1 = getColor();
		Color c2 = c1 * .7f;

		Player player = World.Instance.player;
		Quaternion q = (player != null) ? player.transform.rotation : 
			Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

		for (int i = 0; i < NUM_POLYGONS; i++)
		{
			refreshBackground(i, c1, c2, q);
		}

		Camera.main.backgroundColor = c2;
	}
	
	// Update is called once per frame
	void Update () {
		time = Mathf.MoveTowards(time, nextTime, cycleMovementSpeed);

		for (int i = 0; i < NUM_POLYGONS; i++)
		{
			GameObject g = polygons[i].gameObject;

			Vector3 s = g.transform.localScale;
			if (s.x >= 0.1f && s.y >= 0.1f)
			{
				s.x -= 0.1f;
				s.y -= 0.1f;
				g.transform.localScale = s;
			}

		}
	}

	private Color getColor()
	{
		//Max version
		return new Color(
					Mathf.Min(redMax / 255f, Mathf.Sin(frequency*time) * 0.5f + 0.5f),
					Mathf.Min(greenMax / 255f, Mathf.Sin(frequency*time + 2) * 0.5f + 0.5f),
					Mathf.Min(blueMax / 255f, Mathf.Sin(frequency*time + 4) * 0.5f + 0.5f)
					);
		//Alex version
//		return new Color(
//			Mathf.Min(redMax / 255f, Mathf.Sin(frequency*time) * 0.5f + 0.5f),
//			Mathf.Min(greenMax / 255f, Mathf.Sin(frequency*time + 2) * 0.5f + 0.5f),
//			Mathf.Min(blueMax / 255f, Mathf.Sin(frequency*time + 4) * 0.5f + 0.5f)
//			);
	}

}
