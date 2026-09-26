using UnityEngine;

public class AssetInteraction : MonoBehaviour
{
	[Header("Asset Information")]
	public string assetName = "Test Asset";

	[TextArea]
	public string explanation =
		"This asset is generally managed by the council.";

	public bool councilOwned = true;

	private bool playerNearby;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playerNearby = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playerNearby = false;
		}
	}

	public bool IsPlayerNearby()
	{
		return playerNearby;
	}

	public void SelectAsset()
	{
		GameManager.Instance.OpenAssetQuestion(this);
	}
}