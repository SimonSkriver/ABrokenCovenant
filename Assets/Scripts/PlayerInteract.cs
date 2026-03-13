using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] Transform eyes;
    [SerializeField] float reach = 1f;
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

    [SerializeField] float interactRadius = 0.25f;

    void HandleInteract()
    {
        if (interactAction.WasPressedThisFrame() && PlayerMovement.Instance.currentState == PlayerState.Normal)
        {
            Debug.Log("Interact was called");
            RaycastHit hit;
            bool interacted = false;

            // First try a precise raycast
            if (Physics.Raycast(eyes.position, eyes.forward, out hit, reach))
            {
                interacted = TryInteract(hit);
            }

            // If no hit on precise ray, try a more forgiving spherecast
            if (!interacted && Physics.SphereCast(eyes.position, interactRadius, eyes.forward, out hit, reach))
            {
                interacted = TryInteract(hit);
            }

            // Optionally highlight or log when no target found
            if (!interacted)
            {
                Debug.Log("No interactable target in reach");
            }
        }
        
        else if (interactAction.WasPressedThisFrame() && (PlayerMovement.Instance.currentState == PlayerState.IsCarrying || PlayerMovement.Instance.currentState == PlayerState.InPuzzle))
        {
            Debug.Log("Dropping");
            if (interactable != null)
            {
                interactable.Drop();
            }
        }
    }

    private bool TryInteract(RaycastHit hit)
    {
        var target = hit.collider.GetComponent<IInteractable>();
        if (target != null)
        {
            interactable = target;
            interactable.Interact(hit.collider.gameObject);
            return true;
        }
        return false;
    }
}