using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] Transform eyes;
    [SerializeField] float reach = 3f; //How far is the raycast
    [SerializeField] InputAction interactAction;
    private IInteractable interactable;

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
        //Only handle input if our playerstate is normal
        if (interactAction.WasPressedThisFrame() && PlayerMovement.Instance.currentState == PlayerState.Normal)
        {
            //Spherecast to be more forgiving. Check for an interactable. If there is one, call its interact method
            if (Physics.SphereCast(eyes.position, 0.2f, eyes.forward, out RaycastHit hit, reach))
            {
                interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(hit.collider.gameObject);
                }
            }
        }

        //Only call the interactables' drop, if we are in puzzle or carrying something
        else if (interactAction.WasPressedThisFrame() && (PlayerMovement.Instance.currentState == PlayerState.IsCarrying || PlayerMovement.Instance.currentState == PlayerState.InPuzzle))
        {
            Debug.Log("Dropping");
            interactable.Drop();
        }
    }
}