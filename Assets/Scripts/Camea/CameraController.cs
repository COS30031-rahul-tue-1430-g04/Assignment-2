using UnityEngine;

[RequireComponent(typeof(CameraBounds))]
public class CameraController : MonoBehaviour
{
	public Transform followTransform;
	public float followSpeed = 5f;

	private CameraBounds cameraBounds;
	private void Awake()
	{
		cameraBounds = GetComponent<CameraBounds>();
	}

	private void LateUpdate()
	{
		float t = 1f - Mathf.Exp(-followSpeed * Time.deltaTime);
		Vector2 smoothed = Vector2.Lerp(transform.position, followTransform.position, t);

		float camHalfHeight = Camera.main.orthographicSize;
		float camHalfWidth = camHalfHeight * Camera.main.aspect;

		float clampedX = Mathf.Clamp(smoothed.x, cameraBounds.minBounds.x + camHalfWidth, cameraBounds.maxBounds.x - camHalfWidth);
		float clampedY = Mathf.Clamp(smoothed.y, cameraBounds.minBounds.y + camHalfHeight, cameraBounds.maxBounds.y - camHalfHeight);

		transform.position = new Vector3(clampedX, clampedY, transform.position.z);
	}
}
