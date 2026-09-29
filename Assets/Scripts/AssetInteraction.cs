using UnityEngine;
using UnityEngine.EventSystems;

public class AssetInteraction : MonoBehaviour, IPointerClickHandler
{
	public GameObject player;
	public GameObject star = null;


	public bool alreadySelected = false;


	[Header("Asset Information")]
	public string assetName = "Test Asset";

	[TextArea]
	public string explanation =
		"This asset is generally managed by the council.";

	public bool councilOwned = true;

	public bool SelectAsset()
	{
		// Stop the same asset from being selected again
		if (alreadySelected)
		{
			Debug.Log(name + " has already been completed.");
			return false;
		}

		// Mark this asset as selected
		alreadySelected = true;

		if (star != null)
		{
			star.SetActive(false);
		}

		// Open the question
		GameManager.Instance.OpenAssetQuestion(this);

		Debug.Log(name + " has been selected.");
		return true;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log(name + " has been clicked.");

		if (player != null && PlayerwithinRange(7))
		{
			SelectAsset();
		}
	}

	private bool PlayerwithinRange(int range)
	{
		if (player == null)
			return false;

		Vector3 playerv = player.transform.position;

		float distance =
			Vector2.Distance(playerv, transform.position);

		return distance < range;
	}
}