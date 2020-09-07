using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RigidbodyController : MonoBehaviour
{
	//Camera cam;
	Vector3 inputVec = Vector3.zero;
	Rigidbody body;
	float speed = 2f;
	public Transform playerModel;
	Vector3 lookDir = Vector3.forward;

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

		if (Input.GetButtonDown("Jump"))
		{
			body.velocity = Vector3.up * 5f;
		}
	}
	void FixedUpdate()
	{

		//add stop force
		Vector3 velocity = body.velocity;
		velocity.y = 0; //ignore y
		body.AddForce(-velocity * 0.3f, ForceMode.VelocityChange); //stopping force

		//add WASD force
		body.AddForce(inputVec * speed, ForceMode.VelocityChange);
	}
	void LateUpdate()
	{
		/*if (body.velocity.magnitude > 0)
		{
			lookDir = body.velocity;
			lookDir.y = 0;
			lookDir.Normalize();
		}*/
		if (inputVec.magnitude > 0)
		{
			lookDir = inputVec;
			lookDir.Normalize();
		}
		playerModel.rotation = Quaternion.Slerp(playerModel.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);

	}
}
