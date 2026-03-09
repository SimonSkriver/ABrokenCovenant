using UnityEngine;
using UnityEngine.InputSystem;

public class RotatePuzzle : MonoBehaviour, IInteractable
{
    [SerializeField] Transform playerAnchor;
    [SerializeField] float mouseSens = 1f;
    [SerializeField] Transform horizontalPivot;
    [SerializeField] Transform verticalPivot;
    private InputAction lookAction;
    private PlayerInteract playerInteract;
    
    void Start()
    {
        playerInteract = GetComponent<PlayerInteract>();
        lookAction = InputSystem.actions.FindAction("Look");
    }

    void Update()
    {
        if (PlayerMovement.Instance.currentState == PlayerState.InPuzzle)
        {
            DoPuzzle();
        }
    }

    public void Interact(Transform obj)
    {
        Debug.Log("Youre in puzzle");
        PlayerMovement.Instance.currentState = PlayerState.InPuzzle;
        PlayerMovement.Instance.transform.position = playerAnchor.position;
        //playerInteract.interactAction.
    }

    public void Drop()
    {
        PlayerMovement.Instance.currentState = PlayerState.Normal;
    }

    void DoPuzzle()
    {
        Vector2 mouseInput = lookAction.ReadValue<Vector2>();
        float horizontalInput = mouseInput.x * mouseSens * Time.deltaTime;
        horizontalPivot.transform.rotation = Quaternion.Euler(transform.rotation.x, horizontalInput, transform.rotation.z);
    }
}