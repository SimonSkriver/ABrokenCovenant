using UnityEngine;

public class PickupAndDropItem : MonoBehaviour, IInteractable //Use interactable interface
{
    private Transform hands;
    public GameObject heldObject;

    [SerializeField] AudioSource itemSFX;
    [SerializeField] bool shouldIMakeNoise;

    void Awake()
    {
        hands = GameObject.FindGameObjectWithTag("Hands").transform;

        if (itemSFX != null && shouldIMakeNoise)
        {
            itemSFX.Play();
        }
    }

    public void Interact(GameObject obj)
    {
        //Update playerstate and heldobject variable
        PlayerMovement.Instance.currentState = PlayerState.IsCarrying;
        heldObject = obj;

        //Stop idle sfx
        StopOrStartItemSFX();

        //Disable physics, colliders and parent to hands
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject.GetComponent<BoxCollider>().isTrigger = true;
        heldObject.transform.SetParent(hands);

        //Reset position and rotation relative to hands
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        //Find Transform of held object
        Transform obj = heldObject.GetComponent<Transform>();

        //Start idle sfx again
        StopOrStartItemSFX();

        //Re-enable properties
        obj.transform.parent = null;
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<BoxCollider>().isTrigger = false;

        //Reset objects' rotation, clear variables and reset player state
        obj.localRotation = Quaternion.identity;
        heldObject = null;
        PlayerMovement.Instance.currentState = PlayerState.Normal;
    }

    public void StopOrStartItemSFX()
    {
        if (itemSFX != null && shouldIMakeNoise)
        {
            if (itemSFX.isPlaying)
            {
                itemSFX.Stop();
            }
            else
            {
                itemSFX.Play();
            }
        }
    }
}