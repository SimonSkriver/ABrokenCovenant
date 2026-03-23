using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] Transform eyes;
    [SerializeField] float reach = 3f;
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
        if (interactAction.WasPressedThisFrame() && PlayerMovement.Instance.currentState == PlayerState.Normal)
        {
            if (Physics.SphereCast(eyes.position, 0.2f, eyes.forward, out RaycastHit hit, reach))
            {
                interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(hit.collider.gameObject);
                }
            }
        }

        else if (interactAction.WasPressedThisFrame() && (PlayerMovement.Instance.currentState == PlayerState.IsCarrying || PlayerMovement.Instance.currentState == PlayerState.InPuzzle))
        {
            Debug.Log("Dropping");
            interactable.Drop();
        }
    }
}