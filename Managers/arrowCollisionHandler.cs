using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowCollisionHandler : MonoBehaviour {

	SkeletonAttack skeletonAttack;

	// Use this for initialization
	void Awake () {
		skeletonAttack = GetComponentInParent<SkeletonAttack> ();
	}

	void OnTriggerEnter(Collider other)
	{
		skeletonAttack.HandleCollision (other);
	}
}
