using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	const float EDGE_THRESHOLD = 0.3f;

	public Vector2 localVelocity;
	public float lengthPerDistance;
	public Vector2 centerPosition;
	public BoxCollider2D bulletCollider;

	private NutrientColor color;
	public NutrientColor Color {
		get { return color; }
		set {
			color = value;
//			this.lineRenderer.sharedMaterial.color = color.ColorValue();
			this.lineRenderer.material.color = color.ColorValue();
			this.lineRenderer.SetColors(color.ColorValue(), color.ColorValue());
		}
	}

	public LineRenderer lineRenderer;

	void Start() {
		this.lineRenderer = this.GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update () {
		this.transform.localPosition += (this.localVelocity * Time.deltaTime).ToVector3();
//		Debug.Log (this.Color.ColorValue());

		Vector3 viewportPoint = Camera.main.WorldToViewportPoint(this.transform.position);
		if (viewportPoint.x < -EDGE_THRESHOLD || viewportPoint.x > 1 + EDGE_THRESHOLD ||
		    viewportPoint.y < -EDGE_THRESHOLD || viewportPoint.y > 1 + EDGE_THRESHOLD) {
			GameObject.Destroy(this.gameObject);
		}

		Vector2 distance = this.transform.position.ToVector2() - this.centerPosition;
		float length = lengthPerDistance * distance.magnitude;
		Vector2 unitDir = distance.Rotate90DegreesCounterClockwise().normalized;
		Vector2 v1 = this.transform.position.ToVector2() + unitDir * (length / 2);
		Vector2 v2 = v1 - unitDir * (length + Player.LINE_WIDTH * 0.4f);
		this.lineRenderer.SetPosition(0, v1);
		this.lineRenderer.SetPosition(1, v2);

		this.bulletCollider.size = new Vector2(length, this.bulletCollider.size.y);
	}
}
