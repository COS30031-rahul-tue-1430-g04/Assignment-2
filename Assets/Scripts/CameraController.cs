using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform followTransform;
	public float followSpeed = 5f;

	private void LateUpdate()
	{
		Vector2 targetPosition = Vector2.Lerp(transform.position, followTransform.position, followSpeed * Time.deltaTime);
		transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
	}
}
