using UnityEngine;

public class TerrainZone : MonoBehaviour
{
	[Header("Terrain Settings")]
	[Tooltip("1 = normal speed, 0.5 = half speed, 2 = double speed")]
	public float speedMultiplier = 1f;

	[Header("Water")]
	public bool isWater = false;

	[Header("Terrain Information")]
	public string terrainName = "Ground";

	private void OnTriggerEnter2D(Collider2D other)
	{
		PlayerController player =
			other.GetComponent<PlayerController>();

		if (player == null)
			return;

		player.EnterTerrain(
			this,
			speedMultiplier,
			isWater,
			terrainName
		);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		PlayerController player =
			other.GetComponent<PlayerController>();

		if (player == null)
			return;

		player.ExitTerrain(this);
	}
}