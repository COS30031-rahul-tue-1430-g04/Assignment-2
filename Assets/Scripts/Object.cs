using UnityEngine;
using UnityEngine.EventSystems;


public class Object : MonoBehaviour, IPointerClickHandler
{
    public GameObject player;
	//public GameObject GameManager;

	//types determine whether the object is or isn't publicly managed
	public Types myType = new();
    //chosenTypes determine what the player has selected the object as
    public ChosenTypes myChosenType = new();
    
    public enum Types{publiclyManaged, privatelyManaged};
    public enum ChosenTypes{notChosen, publiclyManaged, privatelyManaged};
    
    public void OnPointerClick(PointerEventData eventData)//happens when object is clicked
    {
        Debug.Log(" has been clicked");
        
        if (PlayerwithinRange(7))
        {
            //OpenAssetQuestion();
            Debug.Log(name + " has been clicked and is within range");
        }
    }

	private bool PlayerwithinRange(int range) //checks player is within selection range
	{
        Vector3 playerv = player.transform.position;
        float distance = Vector2.Distance(playerv, transform.position);
        return distance < range;
    }
}
