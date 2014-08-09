using UnityEngine;
using System.Collections;

/**
 * Singleton that contains various useful things about the world. 
 */
public class World {
	public MonoBehaviour player;

	private static World me = null;

	private World()
	{
		me = this;
	}

	public static World GetInstance()
	{
		if (me == null)
		{
			me = new World();
		}

		return me;
	}
}
