using UnityEngine;
using UnityEngine.EventSystems;


public class AssetInteraction : MonoBehaviour, IPointerClickHandler
{
	public GameObject player;

	public bool alreadySelected = false;//measures wether or not item has already been selected

	[Header("Asset Information")]
	public string assetName = "Test Asset";

	[TextArea]
	public string explanation =
		"This asset is generally managed by the council.";

	public bool councilOwned = true;

	//private bool playerNearby;

	/*private void OnTriggerEnter2D(Collider2D other)
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
	}*/

	public void SelectAsset()
	{
		GameManager.Instance.OpenAssetQuestion(this);
	}
	
	public void OnPointerClick(PointerEventData eventData)//happens when object is clicked
    {
        Debug.Log(this.name + " has been clicked");
        
        if (playerwithinRange(7) && alreadySelected == false)
        {
			alreadySelected = true;
            SelectAsset();
            Debug.Log(this.name + " has been clicked and is within range");
        }
    }

    private bool playerwithinRange(int range) //checks player is within selection range
    {
        Vector3 playerv = player.transform.position;
        float distance = Vector2.Distance(playerv, this.transform.position);
        if (distance < range)
        {
            return (true);
        }else{
            return(false);
        }
    }
}