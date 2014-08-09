using UnityEngine;
using System.Collections;

public class SortParticles : MonoBehaviour {

	public string sortingLayerName = "foreground";
	public int sortingOrder = 2;

	void Start() {
		// Set the sorting layer of the particle system.
		ParticleSystem particleSystem = this.GetComponent<ParticleSystem>();
		particleSystem.renderer.sortingLayerName = sortingLayerName;
		particleSystem.renderer.sortingOrder = sortingOrder;
	}
}