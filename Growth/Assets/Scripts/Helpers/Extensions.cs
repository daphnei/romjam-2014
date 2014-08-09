using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Extensions {

	public static T Pop<T>(this IList<T> items, int index=0) {
		T toRemove = items[index];
		items.RemoveAt(index);
		return toRemove;
	}
}
