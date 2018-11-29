using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCullDistances : MonoBehaviour {

	void Start() {
		Camera camera = GetComponent<Camera>();
		float[] distances = new float[32];
		distances[0] = 25;
		camera.layerCullDistances = distances;
	}
}
