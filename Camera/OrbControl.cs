
using UnityEngine;
using System.Collections;

public class OrbControl : MonoBehaviour {

	public static OrbControl Instance;

	private float vertical;
	public float TurningSpeed = 2.3f;

	void Start ()
	{
		Instance = this;
		vertical = transform.eulerAngles.x;
	}

	void Update ()
	{
		var mouseVertical = Input.GetAxis("Mouse Y");
		vertical = (vertical - TurningSpeed * mouseVertical) % 360f;
		vertical = Mathf.Clamp(vertical, -30, 60);
		transform.localRotation = Quaternion.AngleAxis(vertical, Vector3.right);

	}
}
