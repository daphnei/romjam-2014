using UnityEngine;
using System.Collections;

public class BackgroundPolygon : MonoBehaviour {

	private MeshFilter playerPolygon = null;
	protected MeshFilter thisMesh = null;

	public int defaultNumVertices = 7;

	// Use this for initialization
	void Start () {
		Player player = World.Instance.player;
		if (player != null)
		{
			this.playerPolygon = player.polygon.GetComponent<MeshFilter>();
			this.thisMesh = this.GetComponent<MeshFilter>();
			this.thisMesh.mesh = playerPolygon.mesh;
		}
		else
		{
			Debug.Log("SHOULD NOT GET HERE 1");
			//No player to get a meshfilter from.
			this.thisMesh = this.GetComponent<MeshFilter>();
			this.thisMesh.mesh = PolygonMaker.makeMesh(defaultNumVertices);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if ( playerPolygon != null && Object.ReferenceEquals(thisMesh.mesh, playerPolygon.mesh) ) {
			thisMesh.mesh = playerPolygon.mesh;
		}
	}
}
