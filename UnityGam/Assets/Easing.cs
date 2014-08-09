using UnityEngine;
using System.Collections;

public class Easing {

	// n in (0,1)
	public static float easeSin(float n) {
		return Mathf.Sin(n * (Mathf.PI / 2)); //TODO this
	}

	public static float easeSinInv(float n) {
		return 1 - (easeSin(n));
	}

}
