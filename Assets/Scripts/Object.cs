using UnityEngine;
using UnityEngine.EventSystems;


public class Object : MonoBehaviour, IPointerClickHandler
{
    public types myType = new types();
    
    public enum types{publiclyManaged, privatelyManaged};
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(this.name + " has been clicked");
    }
}
