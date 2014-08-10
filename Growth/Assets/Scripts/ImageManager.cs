using UnityEngine;
using System.Collections;

public class ImageManager {

	static Material[] mats;

	// Use this for initialization
	public static void loadMaterials () {
		mats = Resources.LoadAll<Material>("m");
	}

	public static Material[] updateTexture(int n){
		//Debug.Log("===");
		//Debug.Log(n);
		int index = Mathf.Min(Mathf.Max(0, n), mats.Length);
		//Debug.Log(index);
		return new Material[] { mats[index] };

	}
}
