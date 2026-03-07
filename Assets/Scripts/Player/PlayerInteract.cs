using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public PlayerState currentState = PlayerState.Normal;
    [SerializeField] Transform eyes;
    [SerializeField] Transform hands;
    [SerializeField] float reach = 1f;

    private Transform heldObject;
    private Transform oldObjectAnchor;
    private InputAction interactAction;

    void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        HandleInteract();
    }

    void HandleInteract()
    {
        if (currentState == PlayerState.InPuzzle) //Only handle input, if we're not currently solving a puzzle
        {
            return;
        }
        if (interactAction.WasPressedThisFrame() && !heldObject)
        {
            if (Physics.Raycast(eyes.position, eyes.forward, out RaycastHit hit, reach))
            {
                Debug.Log("Raycast hit");
                if (hit.collider.CompareTag("Pickup"))  Grab(hit.collider.transform);
                else if (hit.collider.CompareTag("Puzzle"))  hit.collider.GetComponent<RotatePuzzle>().BeginPuzzle(this);
            }
        }
        else if (interactAction.WasPressedThisFrame() && heldObject)
        {
            Debug.Log("Dropping");
            Drop();
        }
    }

    void Grab(Transform mirror)
    {
        if (heldObject != null) //Safety check. Exits method if we're already holding something
        {
            return;
        }
        currentState = PlayerState.IsCarrying;
        heldObject = mirror;
        oldObjectAnchor = mirror.parent;

        //Disable physics, colliders and parent to hands
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject.GetComponent<BoxCollider>().enabled = false;
        heldObject.SetParent(hands);

        //Reset position and rotation relative to hands
        heldObject.localPosition = Vector3.zero;
        heldObject.localRotation = Quaternion.identity;
    }

    void Drop()
    {
        if (!heldObject) //Safety check. Exits method if we're not holding anything
        {
            return;
        }
        currentState = PlayerState.Normal;
        Transform mirror = heldObject;
        
        //Re-enable properties
        mirror.SetParent(oldObjectAnchor);
        mirror.GetComponent<Rigidbody>().isKinematic = false;
        mirror.GetComponent<BoxCollider>().enabled = true;

        //Reset mirror's rotation and clear variables
        mirror.localRotation = Quaternion.identity;
        oldObjectAnchor = null;
        heldObject = null;
    }    
}