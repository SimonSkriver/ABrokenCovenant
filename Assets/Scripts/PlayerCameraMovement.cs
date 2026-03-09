using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 10f;
    [SerializeField] Transform playerController;

    private float xRotation = 0f;
    private InputAction lookAction;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        lookAction = InputSystem.actions.FindAction("Look");
    }

    void Update()
    {
        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        
        float mouseX = lookValue.x * mouseSensitivity * 10 * Time.deltaTime;
        float mouseY = lookValue.y * mouseSensitivity * 10 * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerController.Rotate(Vector3.up * mouseX);
    }
}