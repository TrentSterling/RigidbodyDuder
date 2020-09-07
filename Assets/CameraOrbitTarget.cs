using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraOrbitTarget : MonoBehaviour
{
	public float zoomLevel = 10f;
	public float xRot = 0;
	public float yRot = 0;

	public float xRotSmoothed = 0;
	public float yRotSmoothed = 0;

	public float mouseSensivity = 5f;
	public float wheelSensivity = 6f;

	public Transform target;
	public Vector3 offset;

	public SkinnedMeshRenderer playerRenderer;

	void LateUpdate()
	{
		zoomLevel -= Input.GetAxis("Mouse ScrollWheel") * wheelSensivity;

		zoomLevel = Mathf.Clamp(zoomLevel, 0, 45);

		float mouseX = Input.GetAxis("Mouse X") * mouseSensivity;
		float mouseY = -Input.GetAxis("Mouse Y") * mouseSensivity;

		xRot += mouseY;
		yRot += mouseX;

		xRot = Mathf.Clamp(xRot, -45, 85);

		Cursor.lockState = CursorLockMode.Locked;

		xRotSmoothed = Mathf.Lerp(xRotSmoothed, xRot, Time.smoothDeltaTime * 25f);
		yRotSmoothed = Mathf.Lerp(yRotSmoothed, yRot, Time.smoothDeltaTime * 25f);

		transform.rotation = Quaternion.Euler(xRotSmoothed, yRotSmoothed, 0);

		transform.position = offset + target.position + transform.forward * -zoomLevel;

		if (zoomLevel < 1.0f)
		{
			playerRenderer.enabled = false;
		}
		else
		{
			playerRenderer.enabled = true;
		}
	}
}
