﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Extensions {

	public static T Pop<T>(this IList<T> items, int index=0) {
		T toRemove = items[index];
		items.RemoveAt(index);
		return toRemove;
	}

	public static Color ColorValue(this NutrientColor c) {
		switch (c) {
		case NutrientColor.Yellow:
			return new Color(251f/ 255f, 225f / 255f, 0f);
		case NutrientColor.Blue:
			return new Color(19f / 255f, 205f / 255f, 1);
		case NutrientColor.Red:
			return new Color(244f / 255f, 28f / 255f, 84f / 255f);
		case NutrientColor.Green:
			return new Color(110f / 255f, 232f / 255f, 12f / 255f);
		case NutrientColor.Purple:
			return new Color(84f / 255f, 64f / 255f, 168f / 255f);
		default:
			return new Color();
		}
	}
}
