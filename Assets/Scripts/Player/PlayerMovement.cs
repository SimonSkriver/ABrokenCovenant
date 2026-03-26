using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance; //Singleton logic. Lets us refer to this script from elsewhere without needing a reference

    public PlayerState currentState = PlayerState.Normal;

    private InputActionMap playerActionMap;
    private InputAction moveAction;
    private InputAction jumpAction;

    [SerializeField] CharacterController controller;
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float gravity = -9.81f;
    private Vector3 velocity;

    [Header("SFX")]
    [SerializeField] private float currentXPosition;
    [SerializeField] private float oldXPosition;
    [SerializeField] private float distanceSFXTrigger = 0.01f;
    [SerializeField] private float stopDelay = 0.1f;
    [SerializeField] private float lastMoveTime;
    [SerializeField] private AudioSource walkSFX;

    void Awake()
    {
        //Ensure only one instance of PlayerMovement is in the scene at all times
        if (Instance == null)
        {
            Instance = this;
            currentState = PlayerState.Normal;
        }
        else
        {
            Destroy(gameObject);
        }
        //Initial value check. Is used later on to check if sound should be playing
        currentXPosition = transform.position.x + transform.position.z;
        oldXPosition = currentXPosition;
    }

    void Start()
    {
        playerActionMap = InputSystem.actions.FindActionMap("Player");
        playerActionMap.Enable(); // Switch to player action map
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        lastMoveTime = Time.time; // Check if there has been movement within a time period as an extra meassure for sound playing
    }

    void Update()
    {
        //Only handle jumping, movement and sound if we're not in puzzle or locked
        if (currentState != PlayerState.InPuzzle && currentState != PlayerState.LockPlayer)
        {
            HandleJumping();
            HandleMovement();
            HandleSFX();
        }
    }

    void HandleJumping()
    {
        //Jump
        if (controller.isGrounded && jumpAction.IsPressed())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Because physics
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
        oldXPosition = transform.position.x + transform.position.z;
        Vector2 inputValue = moveAction.ReadValue<Vector2>();
        Vector3 horizontalMovement = (transform.right * inputValue.x + transform.forward * inputValue.y) * movementSpeed;
        
        //Combine vertical and horizontal movement
        Vector3 unifiedMovement = horizontalMovement + (velocity.y * Vector3.up);
        controller.Move(unifiedMovement * Time.deltaTime);
        currentXPosition = transform.position.x + transform.position.z;
    }

    void HandleSFX()
    {
        float changeValue = Mathf.Abs(oldXPosition-currentXPosition);

        if (changeValue > distanceSFXTrigger)
        {
            lastMoveTime = Time.time;

            if (!walkSFX.isPlaying)
            {
                walkSFX.Play();
            }
        }
        else if (walkSFX.isPlaying && Time.time - lastMoveTime > stopDelay)
        {
            StopSFX();
        }
    }

    public void StopSFX()
    {
        walkSFX.Stop();
    }
}