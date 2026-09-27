using UnityEngine;

public class CameraBounds : MonoBehaviour
{
	public Vector2 minBounds = new(-10, -10);
	public Vector2 maxBounds = new(10, 10);

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		Vector3 bl = new(minBounds.x, minBounds.y, 0);
		Vector3 tl = new(minBounds.x, maxBounds.y, 0);
		Vector3 tr = new(maxBounds.x, maxBounds.y, 0);
		Vector3 br = new(maxBounds.x, minBounds.y, 0);

		Gizmos.DrawLine(bl, tl);
		Gizmos.DrawLine(tl, tr);
		Gizmos.DrawLine(tr, br);
		Gizmos.DrawLine(br, bl);
	}
}