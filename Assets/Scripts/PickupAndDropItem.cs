using UnityEngine;

public class PickupAndDropItem : MonoBehaviour, IInteractable
{
    [SerializeField] Transform hands;
    
    private Transform heldObject;
    private Transform oldObjectAnchor;

    public void Interact(Transform obj)
    {
        heldObject = obj;
        oldObjectAnchor = obj.parent;

        //Disable physics, colliders and parent to hands
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject.GetComponent<BoxCollider>().enabled = false;
        heldObject.SetParent(hands);

        //Reset position and rotation relative to hands
        heldObject.localPosition = Vector3.zero;
        heldObject.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        Transform obj = heldObject;
        
        //Re-enable properties
        obj.SetParent(oldObjectAnchor);
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<BoxCollider>().enabled = true;

        //Reset mirror's rotation and clear variables
        obj.localRotation = Quaternion.identity;
        oldObjectAnchor = null;
        heldObject = null;
    }
}