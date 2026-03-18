using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 2f)] float mouseSensitivity;
    [SerializeField] private float mirrorSensitivityMultiplier = 0.25f;
    [SerializeField] Transform playerController;

    private float xRotation = 0f;
    private InputAction lookAction;
    
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        lookAction = InputSystem.actions.FindAction("Look");
        lookAction.Enable();
    }
    void Start()
    {
        mouseSensitivity = 0.5f;
    }

    void Update()
    {
        if(PlayerMovement.Instance.currentState != PlayerState.LockPlayer)
        {
        if(PlayerMovement.Instance.currentState != PlayerState.InPuzzle)
            {
            Vector2 lookValue = lookAction.ReadValue<Vector2>();
        
            float mouseX = lookValue.x * mouseSensitivity; //* 10 * Time.deltaTime; // Removed mouseSensitivity * 10
            float mouseY = lookValue.y * mouseSensitivity; //* 10 * Time.deltaTime; // -||-

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerController.Rotate(Vector3.up * mouseX);
            }    
        else
            {    
            Vector2 lookValue = lookAction.ReadValue<Vector2>();
        
            float mouseX = lookValue.x * mouseSensitivity * mirrorSensitivityMultiplier; //* 10 * Time.deltaTime; // Removed mouseSensitivity * 10
            float mouseY = lookValue.y * mouseSensitivity * mirrorSensitivityMultiplier; //* 10 * Time.deltaTime; // -||-

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerController.Rotate(Vector3.up * mouseX);
            }
        }
    }

    public float GetSensitivity()
    {
        return mouseSensitivity;
    }

    public void SetSensitivity(float value)
    {
        mouseSensitivity = value;
    }
}