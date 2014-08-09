using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Helpers {
	public static Vector3 Flatten(this Vector3 vector) {
		return new Vector3(vector.x, vector.y, 0);
	}

	public static Vector2 ToVector2(this Vector3 vector) {
		return new Vector2(vector.x, vector.y);
	}

	public static Vector2 Rotate90DegreesCounterClockwise(this Vector2 vector) {
		return new Vector2(-vector.y, vector.x);
	}

	public static float AbsSubtract(this float value, float subtract) {
		return Mathf.Abs(value) < Math.Abs(subtract) ? 0 : value - Mathf.Sign(value) * subtract;
	}
}
