using UnityEngine;
using System.Collections;

public class ScoreText : MonoBehaviour {
	int score = 0;

	// Use this for initialization
	void Start () {
		float targetX = Camera.main.pixelWidth;
		float targetY = 0;

		this.transform.position = new Vector3(1, 1, 0);
		//this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(targetX, targetY, 0));

		World.Instance.Register(this);

		this.guiText.text = score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Increment(int amount)
	{
		score++;
		this.guiText.text = score.ToString();
	}

	public void Decrement(int amount)
	{
		if (score > 0)
		{
			score--;
			this.guiText.text = score.ToString();
		}
	}

	public void Reset() {
		score = 0;
	}
}
