using UnityEngine;
using UnityEngine.EventSystems;


public class Object : MonoBehaviour, IPointerClickHandler
{
    //types determine whether the object is or isn't publicly managed
    public types myType = new types();
    //chosenTypes determine what the player has selected the object as
    public chosenTypes myChosenType = new chosenTypes();
    
    public enum types{publiclyManaged, privatelyManaged};
    public enum chosenTypes{notChosen, publiclyManaged, privatelyManaged};
    
    public void OnPointerClick(PointerEventData eventData)//happens when object is clicked
    {
        Debug.Log(this.name + " has been clicked");
    }
}
