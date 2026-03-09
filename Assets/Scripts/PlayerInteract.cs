using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] Transform eyes;
    [SerializeField] Transform hands;
    [SerializeField] float reach = 1f;

    private InputAction interactAction;
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
                    PlayerMovement.Instance.currentState = PlayerState.IsCarrying;
                }
            }
        }
        
        else if (interactAction.WasPressedThisFrame() && PlayerMovement.Instance.currentState == PlayerState.IsCarrying)
        {
            Debug.Log("Dropping");
            interactable.Drop();
        }
    }  
}