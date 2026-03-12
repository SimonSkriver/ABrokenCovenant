using UnityEngine;

public class PickupAndDropItem : MonoBehaviour, IInteractable
{
    [SerializeField] Transform hands;
    
    private GameObject heldObject;
    //private Transform oldObjectAnchor;

    public void Interact(GameObject obj)
    {
        PlayerMovement.Instance.currentState = PlayerState.IsCarrying;
        heldObject = obj;
        //oldObjectAnchor = obj.parent;

        //Disable physics, colliders and parent to hands
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject.GetComponent<BoxCollider>().enabled = false;
        heldObject.transform.SetParent(hands);

        //Reset position and rotation relative to hands
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        Transform obj = heldObject.GetComponent<Transform>();
        PlayerMovement.Instance.currentState = PlayerState.Normal;

        //Re-enable properties
        obj.transform.parent = null;
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<BoxCollider>().enabled = true;

        //Reset mirror's rotation and clear variables
        obj.localRotation = Quaternion.identity;
        //oldObjectAnchor = null;
        heldObject = null;
    }
}