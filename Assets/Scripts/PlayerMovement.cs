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
        if (Instance == null)
        {
            Instance = this;
            currentState = PlayerState.Normal;
        }
        else
        {
            Destroy(gameObject);
        }

        currentXPosition = transform.position.x + transform.position.z;
        oldXPosition = currentXPosition;
    }

    void Start()
    {
        playerActionMap = InputSystem.actions.FindActionMap("Player");
        playerActionMap.Enable();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        lastMoveTime = Time.time;
    }

    void Update()
    {
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