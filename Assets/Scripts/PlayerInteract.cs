using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] Transform eyes;
    [SerializeField] Transform hands;
    [SerializeField] float reach = 1f;

    public InputAction interactAction;
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
            if (Physics.Raycast(eyes.position, eyes.forward, out RaycastHit hit, reach))
            {
                interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(hit.transform);
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