using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Singleton that contains various useful things about the world. 
/// </summary>
public class World : SceneSingleton<World> {

	public Player player;

	public void Start() {
		Time.timeScale = 3.0f;
	}

	public void Register(Player player) {
		this.player = player;
	}
}
