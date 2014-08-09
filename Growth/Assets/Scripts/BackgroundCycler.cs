using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundCycler : Pulser {
	const int NUM_LINES = 40;

	public float frequency = 0.1f;
	public float lineDeviance = 20;

	public float minLineColorFraction = 0.6f;
	public float maxLineColorFraction = 0.9f;

	private MeshFilter filter;
	
	private List<GameObject> lines;
	
	// Use this for initialization
	void Start () {
		base.Start();
		
		this.lines = new List<GameObject>();

		chooseBackgroundColor();

		//Create a bunch of random vertical-ish lines.
//		float approxDistBetweenLines = Camera.main.pixelWidth / NUM_LINES;
//		for (int i = 0; i < NUM_LINES; i++)
//		{
//			float approxXPos = (approxDistBetweenLines * i) + (approxDistBetweenLines/2);
//			createLine(approxXPos);
//		}
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

	private void createLine(float approxXPos)
	{
		GameObject go = new GameObject();
		this.lines.Add(go);
		
		LineRenderer lr = go.AddComponent<LineRenderer>();
		lr.useWorldSpace = false;
		lr.SetVertexCount(2);
		lr.material.color = Color.red;

		//Make sure they are the correct color.
		float fract = Random.Range(minLineColorFraction, maxLineColorFraction);
		lr.material.color = Camera.main.backgroundColor * fract;

		this.updateLinePos(lr, approxXPos);
	}
	
	private void updateLinePos(LineRenderer lr, float approxXPos)
	{
		float lineXPos1 = Random.Range(approxXPos - lineDeviance, approxXPos + lineDeviance);
		float lineXPos2 = Random.Range(lineXPos1 - lineDeviance, lineXPos1 + lineDeviance);
		
		Vector2 startPos = Camera.main.ScreenToWorldPoint(new Vector2(lineXPos1, 0));
		Vector2 endPos = Camera.main.ScreenToWorldPoint(new Vector2(lineXPos2, Camera.main.pixelHeight));
		
		lr.SetPosition(0, startPos);
		lr.SetPosition(1, endPos);
		lr.SetWidth(0.2f, 0.2f);
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
