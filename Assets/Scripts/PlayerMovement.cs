using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    public PlayerState currentState = PlayerState.Normal;

    private InputActionMap playerActionMap;
    private InputAction moveAction;
    private InputAction jumpAction;

    [SerializeField] CharacterController controller;
    [SerializeField] float movementSpeed = 4f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float gravity = -9.81f;
    private Vector3 velocity;

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
        playerActionMap = InputSystem.actions.FindActionMap("Player");
        playerActionMap.Enable();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        if (currentState != PlayerState.InPuzzle && currentState != PlayerState.LockPlayer)
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