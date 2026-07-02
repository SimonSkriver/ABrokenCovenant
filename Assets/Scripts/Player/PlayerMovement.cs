using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance; //Singleton logic. Lets us refer to this script from elsewhere without needing a reference

    public PlayerState currentState = PlayerState.Normal;

    private InputActionMap playerActionMap;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    [SerializeField] CharacterController controller;
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float sprintMultiplier = 1.7f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float gravity = -9.81f;
    public bool isSprinting { get; private set; }
    private Vector3 velocity;

    [Header("SFX")]
    [SerializeField] private float currentPosition;
    [SerializeField] private float oldPosition;
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
        currentPosition = transform.position.x + transform.position.z;
        oldPosition = currentPosition;
    }

    void Start()
    {
        playerActionMap = InputSystem.actions.FindActionMap("Player");
        playerActionMap.Enable(); // Switch to player action map
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
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
        float speed = movementSpeed;
        if (sprintAction.IsPressed())
        {
            isSprinting = true;
            speed *= sprintMultiplier;
        }
        else
        {
            isSprinting = false;
            speed = movementSpeed;
        }
        oldPosition = transform.position.x + transform.position.z;
        Vector2 inputValue = moveAction.ReadValue<Vector2>();
        Vector3 horizontalMovement = (transform.right * inputValue.x + transform.forward * inputValue.y) * speed;
        
        //Combine vertical and horizontal movement
        Vector3 unifiedMovement = horizontalMovement + (velocity.y * Vector3.up);
        controller.Move(unifiedMovement * Time.deltaTime);
        currentPosition = transform.position.x + transform.position.z;
    }

    void HandleSFX()
    {
        // If player has moved save the difference between old and new position as an absolute number
        float changeValue = Mathf.Abs(oldPosition-currentPosition);

        // Check if the movement detected is bigger than distance threshold
        if (changeValue > distanceSFXTrigger)
        {
            // Register the time now that we moved
            lastMoveTime = Time.time;

            // If its not already playing the SFX start it
            if (!walkSFX.isPlaying)
            {
                walkSFX.Play();
            }
        }

        // Otherwise if no change detected, it must mean we stopped moving.
        // If the SFX was playing and the current time minus last move time 
        // is greater than the threshold stop the SFX, this was added as it 
        // many times stopped the final footstep SFX from getting cut off
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