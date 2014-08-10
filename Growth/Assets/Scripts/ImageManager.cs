using UnityEngine;
using System.Collections;

public class ImageManager : MonoBehaviour {

	Material[] mats;
	PolygonMaker maker;
	MeshRenderer render;

	// Use this for initialization
	void Start () {
		mats = Resources.LoadAll<Material>("m");
		maker = this.GetComponent<PolygonMaker>();
		render = this.GetComponent<MeshRenderer>();
	}

	public void updateTexture(int n){
		Debug.Log("===");
		Debug.Log(n);
		int index = Mathf.Min(Mathf.Max(0, n), mats.Length);
		Debug.Log(index);
		renderer.materials = new Material[] { mats[index] };

	}

	// Update is called once per frame
	void Update () {
	}
}
