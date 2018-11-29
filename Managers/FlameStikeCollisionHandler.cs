using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameStikeCollisionHandler : MonoBehaviour {

	public ShootFlameStrike shootFlameStrike;
	public CapsuleCollider playerCollider;

	void OnTriggerEnter(Collider other)
	{
		if (!other.isTrigger && other != playerCollider)
			shootFlameStrike.HandleCollision(other);
	}
}
