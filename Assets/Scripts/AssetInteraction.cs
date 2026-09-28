using UnityEngine;
using UnityEngine.EventSystems;

public class AssetInteraction : MonoBehaviour, IPointerClickHandler
{
	public GameObject player;

	public bool alreadySelected = false;

	[Header("Asset Information")]
	public string assetName = "Test Asset";

	[TextArea]
	public string explanation =
		"This asset is generally managed by the council.";

	public bool councilOwned = true;

	public void SelectAsset()
	{
		// Stop the same asset from being selected again
		if (alreadySelected)
		{
			Debug.Log(this.name + " has already been completed.");
			return;
		}

		// Mark this asset as selected
		alreadySelected = true;

		// Open the question
		GameManager.Instance.OpenAssetQuestion(this);

		Debug.Log(this.name + " has been selected.");
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log(this.name + " has been clicked.");

		if (player != null && playerwithinRange(7))
		{
			SelectAsset();
		}
	}

	private bool playerwithinRange(int range)
	{
		if (player == null)
			return false;

		Vector3 playerv = player.transform.position;

		float distance =
			Vector2.Distance(playerv, this.transform.position);

		return distance < range;
	}
}