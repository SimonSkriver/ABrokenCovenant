using UnityEngine;

public class MirrorSlot : MonoBehaviour
{
    [SerializeField] Transform mirrorSocket;

    // If a mirror enters the trigger collider, call SlotMirror() using the specific mirror as parameter
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mirror")) 
        {
            SlotMirror(other.transform);
        }
    }
    //  Clear the mirrors' held object variable, reset position and rotation after parenting and reset player state
    void SlotMirror(Transform mirror)
    {
        mirror.GetComponent<PickupAndDropItem>().heldObject = null;
        mirror.SetParent(mirrorSocket);
        mirror.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        PlayerMovement.Instance.currentState = PlayerState.Normal;
    }
}