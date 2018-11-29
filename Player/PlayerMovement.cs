using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public static PlayerMovement Instance;

	public int state;
	public float speed = 2f;
	bool IsAiming; 
	float horizontal;
	public float TurningSpeed = 3.0f;

	Animator anim;
	Vector3 centerPoint;
	Vector3 movement;
	Vector3 movementNormalized;
	CursorLockMode lockedMouse;
	Rigidbody playerrigidbody;
	AudioSource[] playerAudio;

	float timer;
	bool jumping;

	public bool blockControl;

	public Camera camera; 

	void Start ()
	{
		Instance = this;
		centerPoint.x = 0.5f;
		centerPoint.y = 0.5f;
		centerPoint.z = 0f;
		anim = GetComponent<Animator>();
		playerrigidbody = GetComponent<Rigidbody> ();
		playerAudio = GetComponents<AudioSource> ();

		state = 0;
		IsAiming = false;
		blockControl = false;
		horizontal = transform.eulerAngles.y;	

	}

	void Update ()
	{
		FocusRaycast();
		if (blockControl == false)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			Control();
		}
		Move ();
		Animate();
		FocusCamera();

		if (state != 0 && !playerAudio[0].isPlaying)
			playerAudio[0].Play ();
	}

	private void Move()
	{
		var mouseHorizontal = Input.GetAxis("Mouse X");
		horizontal = (horizontal + TurningSpeed * mouseHorizontal) % 360f;
		transform.rotation = Quaternion.AngleAxis(horizontal, Vector3.up);

		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");
		movement.Set (h, 0f, v);
		movementNormalized = movement.normalized;
		movement = movementNormalized * speed * Time.deltaTime;

		if (movement.x != 0) {
			//To Compensate for animation
			movement.z *= .48f * speed;
		}

		transform.Translate (movement);

		//timer2 += Time.deltaTime;
		//print (Vector3.Distance (offset, transform.position) / timer2);

		//if (movementNormalized.x == 0.7f)
		//	state = 4;
		//else if (movementNormalized.x == -0.7f)
		//	state = 5;
	}

	private void Animate()
	{
		anim.SetInteger("state", state);
	}

	private void Control()
	{	
		/*
        state:
        01 = Walking
        02 = Running
        03 = Walking Back
        04 = Walking Right
        05 = Walking Left
        06 = Standing Jump
        07 = Running Jump
        */

		if (Input.GetKeyDown("w"))
		{
			state = 1;
		}
		if (Input.GetKeyUp("w") && state == 1)
		{
			state = 0;
			if (Input.GetKey("s")) { state = 3; }
			if (Input.GetKey("a")) { state = 5; }
			if (Input.GetKey("d")) { state = 4; }
		}
		/*if (Input.GetKeyUp("w") && state == 2)
		{
			state = 0;
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && state == 1)
		{
			state = 2;
			if (IsAiming == true)
			{
				IsAiming = false;
			}
		}

		if (Input.GetKeyUp(KeyCode.LeftShift) && state == 2) { state = 1; }
		*/

		if (Input.GetKeyDown("s"))
		{
			state = 3;	
		}
		if (Input.GetKeyUp("s") && state == 3)
		{
			state = 0;
			if (Input.GetKey("a")) { state = 5; }
			if (Input.GetKey("d")) { state = 4; }
			if (Input.GetKey("w")) { state = 1; }
		}

		if (Input.GetKey("d"))
		{
			state = 4;
		}
		if (Input.GetKeyUp("d") && state == 4)
		{
			state = 0;
			if (Input.GetKey("s")) { state = 3; }
			if (Input.GetKey("a")) { state = 5; }
			if (Input.GetKey("w")) { state = 1; }

		}

		if (Input.GetKey("a"))
		{
			state = 5;
		}
		if (Input.GetKeyUp("a") && state == 5)
		{
			state = 0;
			if (Input.GetKey("s")) { state = 3; }
			if (Input.GetKey("d")) { state = 4; }
			if (Input.GetKey("w")) { state = 1; }
		}
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			IsAiming = true;
			if (state == 2)
			{
				state = 1;
			}
		}
		if (Input.GetKeyUp(KeyCode.Mouse1)) { IsAiming = false; }
		
		//Implement Jumping
		/*if (state == 0 && Input.GetKeyDown(KeyCode.Space))
		{
			jumping = true;
			timer = 0;
			state = 6;
		}
		if (jumping && timer < 1.407)
		{
			state = 6;
			timer += Time.deltaTime;
		}
		if (jumping && timer >= 1.407)
		{
			jumping = false;
			state = 0;
		}*/

	}

	private void FocusCamera()
	{
		if (IsAiming == true && camera.fieldOfView > 37)
		{
			camera.fieldOfView = camera.fieldOfView - 65.0f * Time.deltaTime;
		}
		if (IsAiming == false && camera.fieldOfView < 60)
		{
			camera.fieldOfView = camera.fieldOfView + 65.0f * Time.deltaTime;
		}
	}

	private void FocusRaycast()
	{
		RaycastHit hitInfo;
		Ray cameraRay = camera.ViewportPointToRay(centerPoint);
	}


	public bool RetornaIsAiming()
	{
		return IsAiming;
	}
}
