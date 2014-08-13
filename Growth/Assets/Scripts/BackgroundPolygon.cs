using UnityEngine;
using System.Collections;

public class BackgroundPolygon : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

	public void SetMesh(Mesh newMesh)
	{
		MeshFilter mf = this.GetComponent<MeshFilter>();
		mf.sharedMesh = newMesh;
	}

	// Update is called once per frame
	void Update () {
	}
}
