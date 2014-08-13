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
	public BackgroundCycler background;

	public void Register(Player player) {
		this.player = player;
	}

	public void Register(ScoreText score)
	{
		this.score = score;
	}

	public void Register(BackgroundCycler background)
	{
		this.background = background;
	}
}

