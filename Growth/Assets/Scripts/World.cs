using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Singleton that contains various useful things about the world. 
/// </summary>
public class World : SceneSingleton<World> {

	public Player player;
	public ScoreText score;

	public void Register(Player player) {
		this.player = player;
	}

	public void Register(ScoreText score)
	{
		this.score = score;
	}

	/// <summary>
	/// finds and destroys all onscreen particles.
	/// </summary>
	public void ClearScreen() {
		Debug.Log("KILL MEE");
		FreeNutrient[] fs = UnityEngine.Object.FindObjectsOfType<FreeNutrient>();
		foreach (FreeNutrient f in fs) {
			f.PrettyKill();
		}
	}
}
