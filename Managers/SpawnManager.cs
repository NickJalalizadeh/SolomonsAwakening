using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	Transform[] spawnPoints;
	public Transform player;

	void Start () {
		spawnPoints = GetComponentsInChildren<Transform> ();

		player.position = spawnPoints [Random.Range (1, spawnPoints.Length)].position;
	}
}
