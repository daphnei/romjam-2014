using UnityEngine;
using System.Collections;

public class BackgroundPolygon : MonoBehaviour {

	MeshFilter playerPolygon;
	MeshFilter thisMesh;

	// Use this for initialization
	void Start () {
		playerPolygon = GameObject.Find("Player/Polygon").GetComponent<MeshFilter>();
		thisMesh = this.GetComponent<MeshFilter>();
		thisMesh.mesh = playerPolygon.mesh;
	}
	
	// Update is called once per frame
	void Update () {
		if ( Object.ReferenceEquals(thisMesh.mesh, playerPolygon.mesh) ) {
			thisMesh.mesh = playerPolygon.mesh;
		}
	}
}
