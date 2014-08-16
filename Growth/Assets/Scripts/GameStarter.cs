using UnityEngine;
using System.Collections;

public class GameStarter : MonoBehaviour {

	public EnemyGenerator gen;

	Renderer[] renderers;
	bool vis = true;

	static float timetopause = 5f;
	private float elapsed = 0;

	// Use this for initialization
	void Start () {
		renderers = gameObject.GetComponentsInChildren<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		elapsed += Time.deltaTime;
		if (Input.GetMouseButton(0)) {
			elapsed = 0;
		}
		if (vis && Input.GetMouseButtonDown(0)) {
			setVisibility(vis = false);
			gen.timeline.RestartTimeline();
		}
		else if (!vis && World.Instance.player.CurVerts() == 3 && elapsed > timetopause) {
			Debug.Log("Done here");
			//reset (with screen clear?)
			setVisibility(vis = true);
			gen.timeline.PauseTimeline();
			World.Instance.score.Reset();
			World.Instance.ClearScreen();
			World.Instance.player.ResetNutrients();
			//TODO also reset the nutirent count of player
		}
	}

	void setVisibility(bool vis) {
		foreach (Renderer r in renderers) {
			r.enabled = vis;
		}
	}
}
