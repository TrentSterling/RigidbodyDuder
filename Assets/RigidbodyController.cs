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
	public float coyoteTime = 0;
	public Collider physCollider;
	public PhysicMaterial slipperyMaterial;
	public PhysicMaterial stickyMaterial;
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

		if (inputVec.magnitude > 1)
		{
			inputVec.Normalize();
		}

		//set rotation direction if input detected
		if (inputVec.magnitude > 0)
		{
			lookDir = inputVec;
			lookDir.Normalize();
		}

		//do a jumppy
		if (Input.GetButtonDown("Jump"))
		{
			if (coyoteTime > 0)
			{
				//body.velocity = Vector3.up * 7.5f;
				physCollider.sharedMaterial = slipperyMaterial;
				body.drag = 0f;
				body.AddForce(Vector3.up * -body.velocity.y, ForceMode.VelocityChange);
				body.AddForce(Vector3.up * 7.5f, ForceMode.VelocityChange);
				//Vector3 vel = body.velocity;
				//vel.y = 7.5f;
				//body.velocity = vel;
				//body.velocity = Vector3.up * 7.5f;
				grounded = false;
			}
		}
		if (Input.GetButton("Jump"))
		{
			coyoteTime = 0;
		}

	}
	void FixedUpdate()
	{
		DetectGround(); //set jump stuff
		if (grounded)
		{

			//add stop force
			Vector3 velocity = body.velocity;
			velocity.y = 0; //ignore y
			body.AddForce(-velocity * 0.2f, ForceMode.VelocityChange); //stopping force
			//affect drag via input.magnitude

			if (Input.GetKey(KeyCode.LeftShift))
			{

				//add WASD force
				body.AddForce(inputVec * speed * 2, ForceMode.VelocityChange);
			}
			else
			{
				//add WASD force
				body.AddForce(inputVec * speed, ForceMode.VelocityChange);

			}
		}
		else
		{

			Vector3 velocity = body.velocity;
			velocity.y = 0; //ignore y
			body.AddForce(-velocity * 0.04f, ForceMode.VelocityChange); //stopping force
			body.AddForce(inputVec * speed * 0.1f, ForceMode.VelocityChange);
		}
	}
	void LateUpdate()
	{
		playerModel.rotation = Quaternion.Slerp(playerModel.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);
	}
	bool grounded = false;
	public void DetectGround()
	{
		//		if (Physics.Raycast(transform.position + Vector3.up * 0.01f, Vector3.down, 0.03f, groundLayers))
		RaycastHit hit;

		if (Physics.SphereCast(transform.position + Vector3.up * 0.1f, 0.1f, Vector3.down, out hit, 0.2f, groundLayers))
		{
			coyoteTime = 0.04f; //tweak this
			grounded = true;
			//physCollider.sharedMaterial = stickyMaterial;
			body.drag = 5f;
		}
		else
		{
			grounded = false;
			physCollider.sharedMaterial = slipperyMaterial;
			body.drag = 0f;
		}
	}
}
