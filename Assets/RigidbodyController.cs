using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RigidbodyController : MonoBehaviour
{
	//Camera cam;
	public Transform playerModel;
	public float speed = 2f;

	public float stopStrength = 0.3f;
	public LayerMask groundLayers;

	Vector3 inputVec = Vector3.zero;
	Rigidbody body;
	Vector3 lookDir = Vector3.forward;
	float coyoteTime = 0;

	void Start()
	{
		body = GetComponent<Rigidbody>();
	}
	void Update()
	{
		inputVec.x = Input.GetAxis("Horizontal");
		inputVec.z = Input.GetAxis("Vertical");

		//get cam rotated input
		inputVec = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * inputVec;

		//set rotation direction if input detected
		if (inputVec.magnitude > 0)
		{
			lookDir = inputVec;
			lookDir.Normalize();
		}

		//do a jumppy
		if (Input.GetButtonDown("Jump") && coyoteTime > 0)
		{
			coyoteTime = 0;
			body.velocity = Vector3.up * 7.5f;
		}
	}
	void FixedUpdate()
	{
		DetectGround(); //set jump stuff

		//add stop force
		Vector3 velocity = body.velocity;
		velocity.y = 0; //ignore y
		body.AddForce(-velocity * 0.3f, ForceMode.VelocityChange); //stopping force

		//add WASD force
		body.AddForce(inputVec * speed, ForceMode.VelocityChange);
	}
	void LateUpdate()
	{
		playerModel.rotation = Quaternion.Slerp(playerModel.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);
	}
	public void DetectGround()
	{
		if (Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayers))
		{
			coyoteTime = 0.2f; //tweak this
		}
	}
}
