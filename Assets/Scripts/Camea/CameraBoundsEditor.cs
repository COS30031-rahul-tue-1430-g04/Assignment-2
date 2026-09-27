using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraBounds))]
public class CameraBoundsEditor : Editor
{
	private void OnSceneGUI()
	{
		CameraBounds bounds = (CameraBounds)target;

		Handles.color = Color.cyan;

		Vector3 min = new(bounds.minBounds.x, bounds.minBounds.y, 0);
		Vector3 max = new(bounds.maxBounds.x, bounds.maxBounds.y, 0);

		Vector3 newMin = Handles.FreeMoveHandle(
			min, 0.2f, Vector3.zero, Handles.SphereHandleCap
		);

		// Drag top-right
		Vector3 newMax = Handles.FreeMoveHandle(
			max, 0.2f, Vector3.zero, Handles.SphereHandleCap
		);

		if (newMin != min || newMax != max)
		{
			Undo.RecordObject(bounds, "Move Camera Bounds");
			bounds.minBounds = (Vector2)newMin;
			bounds.maxBounds = (Vector2)newMax;
		}
	}
}
