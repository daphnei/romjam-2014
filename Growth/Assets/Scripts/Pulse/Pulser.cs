using UnityEngine;
using System.Collections;

public class Pulser : MonoBehaviour  {

	protected virtual void Start() {
		PulseController.Instance.AddPulser(this);
	}

	protected virtual void OnDestroy() {
		if (PulseController.Instance != null)
			PulseController.Instance.RemovePulser(this);
	}

	public virtual void Pulse() {
	}
}
