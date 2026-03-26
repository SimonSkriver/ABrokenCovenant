using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField, Range(0.05f, 1f)] float mouseSensitivity;
    [SerializeField] private float mirrorSensitivityMultiplier = 0.25f;
    [SerializeField] Transform playerController;

    private float xRotation = 0f;
    private InputAction lookAction;
    
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lookAction = InputSystem.actions.FindAction("Look");
        lookAction.Enable();
    }
    
    void Start()
    {
        mouseSensitivity = 0.33f;
    }

    void Update()
    {
        //Only handle input if we aren't locked or InPuzzle
        if(PlayerMovement.Instance.currentState != PlayerState.LockPlayer)
        {
            if(PlayerMovement.Instance.currentState != PlayerState.InPuzzle)
            {
                //Read input and save in lookValue
                Vector2 lookValue = lookAction.ReadValue<Vector2>();
        
                float mouseX = lookValue.x * mouseSensitivity; 
                float mouseY = lookValue.y * mouseSensitivity; 

                //Rotation is applied to the player camera based on input, and is clamped between 0 and 90 on the x axis
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerController.Rotate(Vector3.up * mouseX);
            }    
            else
            {    
                //Same logic as above but when in puzzle, reduce the sensitivity 
                Vector2 lookValue = lookAction.ReadValue<Vector2>();
        
                float mouseX = lookValue.x * mouseSensitivity * mirrorSensitivityMultiplier;
                float mouseY = lookValue.y * mouseSensitivity * mirrorSensitivityMultiplier; 

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerController.Rotate(Vector3.up * mouseX);
            }
        }
    }

    //Get and set sensitivity to be used in options menu
    public float GetSensitivity()
    {
        return mouseSensitivity;
    }

    public void SetSensitivity(float value)
    {
        mouseSensitivity = value;
    }
}