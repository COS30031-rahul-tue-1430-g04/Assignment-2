#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		// Draw the normal GameManager Inspector
		DrawDefaultInspector();

		EditorGUILayout.Space(15);

		// Get GameManager
		GameManager gameManager =
			(GameManager)target;

		EditorGUILayout.LabelField(
			"Assets Found in This Level",
			EditorStyles.boldLabel
		);

		EditorGUILayout.Space(5);

		// Find all AssetInteraction components
		AssetInteraction[] assets =
			FindObjectsByType<AssetInteraction>(
				FindObjectsInactive.Include,
				FindObjectsSortMode.None
			);

		// Display total
		EditorGUILayout.HelpBox(
			"Total Assets Found: " +
			assets.Length,
			MessageType.Info
		);

		EditorGUILayout.Space(5);

		// Display every asset
		for (int i = 0; i < assets.Length; i++)
		{
			AssetInteraction asset = assets[i];

			EditorGUILayout.BeginHorizontal();

			// Asset number
			EditorGUILayout.LabelField(
				(i + 1) + ".",
				GUILayout.Width(30)
			);

			// Asset name
			EditorGUILayout.LabelField(
				asset.gameObject.name,
				GUILayout.Width(180)
			);

			// Asset type/name
			EditorGUILayout.LabelField(
				asset.assetName,
				GUILayout.Width(150)
			);

			// Active status
			string status =
				asset.gameObject.activeInHierarchy
					? "Active"
					: "Inactive";

			EditorGUILayout.LabelField(
				status,
				GUILayout.Width(70)
			);

			// Select button
			if (GUILayout.Button(
				"Select",
				GUILayout.Width(60)
			))
			{
				Selection.activeGameObject =
					asset.gameObject;

				EditorGUIUtility.PingObject(
					asset.gameObject
				);
			}

			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.Space(10);

		// Refresh button
		if (GUILayout.Button(
			"Refresh Asset List"
		))
		{
			EditorUtility.SetDirty(
				gameManager
			);

			Repaint();
		}
	}
}

#endif