using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour {

	ShootFireBall shootFireBall;
	ShootFlameStrike shootFlameStike;

	void Awake() {
		
		shootFireBall = GetComponentInChildren<ShootFireBall> ();
		shootFlameStike = GetComponentInChildren<ShootFlameStrike> ();
	}

	void ShootFB () {
		shootFireBall.ShootFB ();
	}
	void ShootFS () {
		shootFlameStike.ShootFS ();
	}
}
