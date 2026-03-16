using UnityEngine;

public class ChurchDoor : MonoBehaviour, IInteractable
{
    public void Interact(GameObject obj)
    {
        Debug.Log("You won");
    }

    public void Drop()
    {
        
    }
}