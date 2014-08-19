using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	int score = 0;
	int scoreMultiplier = 0;

	//This is a GuiText because it doesn't need to be resized or anything fancy.
	public GUIText pointsText;

	//This is a text mesh because I want to animate it when it changes.
	public TextMesh multiplierText;


	private const float SCALE_INC_VALUE = 0.01f;
	private float targetMultiplierTextScale = 0.05f;

	// Use this for initialization
	void Start () {
		float targetX = Camera.main.pixelWidth;
		float targetY = 0;

		//Set the position of the points text.
		this.pointsText.gameObject.transform.position = new Vector3(0.5f, 0, 0);

		//Set the position of the multiplier text.
		Vector3 targetPos = Camera.main.ViewportToWorldPoint(new Vector2(.5f, .98f));
		targetPos.z = 0;
		this.multiplierText.transform.position = targetPos;

		this.multiplierText.transform.localScale = Vector2.one * this.targetMultiplierTextScale;

		//The points text remains a static size.
		this.pointsText.fontSize = Mathf.Min(Screen.height, Screen.width)/20;

		World.Instance.Register(this);

		this.pointsText.text = score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 scale = this.multiplierText.transform.localScale;
		if (scale.x > this.targetMultiplierTextScale && !Mathf.Approximately(scale.x, this.targetMultiplierTextScale))
		{
			scale.x -= SCALE_INC_VALUE;
			scale.y -= SCALE_INC_VALUE;
			this.multiplierText.transform.localScale = scale;
		}

	}

	public void Increment(int amount)
	{
		this.scoreMultiplier++;
		this.updateMultiplierText();

		startMultiplierPulse(0);

		this.score += (amount * this.scoreMultiplier);

		this.pointsText.text = score.ToString();
	}

	public void Decrement(int amount)
	{
		//this.scoreMultiplier = 0;
		this.updateMultiplierText();

		if (this.score > 0)
		{
			this.score--;
			this.pointsText.text = score.ToString();
		}

		//this.multiplierText.transform.localScale = Vector2.one * 0.05f;

	}

	public void Reset() {
		score = 0;
		this.pointsText.text = score.ToString();
	}

	private void startMultiplierPulse(int targetSize)
	{
		Vector3 scale = this.multiplierText.transform.localScale ;

		this.targetMultiplierTextScale = Mathf.Min(0.2f, scale.x + SCALE_INC_VALUE);

		scale.x += (SCALE_INC_VALUE * 8);
		scale.y += (SCALE_INC_VALUE * 8);
		this.multiplierText.transform.localScale = scale;
	}

	private void updateMultiplierText()
	{
		this.multiplierText.text = "Combo x" + this.scoreMultiplier.ToString();
	}
}
