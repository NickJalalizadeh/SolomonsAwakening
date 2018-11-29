using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoltCollisionHandler : MonoBehaviour {

	public ShootFireBolt shootFireBolt;
	public CapsuleCollider playerCollider;


	void OnTriggerEnter(Collider other)
	{
		if (!other.isTrigger && other != playerCollider)
			shootFireBolt.HandleCollision(other);
	}
}
