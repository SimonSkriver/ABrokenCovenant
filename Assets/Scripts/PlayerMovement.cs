using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    public PlayerState currentState = PlayerState.Normal;

    InputAction moveAction;
    InputAction jumpAction;

    [SerializeField] CharacterController controller;
    [SerializeField] float movementSpeed = 4f; //potentially make two speed values. One for forward and one for left/right, so that strafing is a bit slower?
    [SerializeField] float jumpHeight = 3f;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            currentState = PlayerState.Normal;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        if (currentState != PlayerState.InPuzzle)
        {
            HandleJumping();
            HandleMovement();
        }
    }

    void HandleJumping()
    {
        //Jump
        if (controller.isGrounded && jumpAction.IsPressed())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //fiddle with jump so that it's less floaty. Needs to jump faster without jumping higher
        }

        //Keep player grounded, by forcing slight negative downward velocity
        if(controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Apply gravity
        velocity.y += gravity * Time.deltaTime;
    }

    void HandleMovement()
    {
        //Movement
        Vector2 inputValue = moveAction.ReadValue<Vector2>();
        Vector3 horizontalMovement = (transform.right * inputValue.x + transform.forward * inputValue.y) * movementSpeed;
        
        //Combine vertical and horizontal movement
        Vector3 unifiedMovement = horizontalMovement + (velocity.y * Vector3.up);
        controller.Move(unifiedMovement * Time.deltaTime);
    }
}