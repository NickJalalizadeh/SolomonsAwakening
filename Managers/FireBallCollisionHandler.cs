using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCollisionHandler : MonoBehaviour {

	public ShootFireBall shootFireBall;
	public CapsuleCollider playerCollider;

	void OnTriggerEnter(Collider other)
	{
		if (!other.isTrigger && other != playerCollider)
			shootFireBall.HandleCollision(other);
	}
}
