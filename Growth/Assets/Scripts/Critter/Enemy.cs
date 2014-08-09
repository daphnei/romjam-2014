using UnityEngine;
using System.Collections;

public class Enemy : Critter {

	protected override void HitThePlayer ()
	{
		base.HitThePlayer ();

		World.Instance.player.RemoveNutrient();
	}
}
