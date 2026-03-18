using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsMenuToggle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject volumeSettingsPanel;
    [SerializeField] private GameObject mouseSensPanel;
    //[SerializeField] private GameObject buttonSettings;
    [SerializeField] private InputAction escapeAction;
    [SerializeField] private PlayerCameraMovement playerCameraMovement;

    [Header("Sensitivity settings")]
    [SerializeField] private Slider sensitivitySlider;

    [Header("Checks")]
    [SerializeField] private bool escapeMenuShown = false;
    [SerializeField] private bool volumeSettingsShown = false;

    PlayerState savedPlayerState;

    void Awake()
    {
        escapeAction = InputSystem.actions.FindAction("Escape");
        if (escapeAction != null)
        {
            escapeAction.Enable();
            escapeAction.performed += ctx => ShowAndHidePauseScreen();
        }
        if (playerCameraMovement != null)
        {
            sensitivitySlider.value = playerCameraMovement.GetSensitivity();
        }
    }
    public void ShowAndHidePauseScreen()
    {
        if(!escapeMenuShown) 
        {
            savedPlayerState = PlayerMovement.Instance.currentState;
            PlayerMovement.Instance.currentState = PlayerState.LockPlayer;
            pausePanel.SetActive(true);
            //buttonSettings.SetActive(false);
            Time.timeScale = 0f;
            escapeMenuShown = true;
            mouseSensPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }

        else if(escapeMenuShown)
        {
            Cursor.lockState = CursorLockMode.Locked;
            //Ensure revert panel if player exits to game while in volume settings via Escape
            volumeSettingsPanel.SetActive(false);
            volumeSettingsShown = false;

            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            escapeMenuShown = false;
            if (savedPlayerState == PlayerState.InPuzzle) 
            {
                PlayerMovement.Instance.currentState = PlayerState.InPuzzle;
            }
            else
            {
               PlayerMovement.Instance.currentState = PlayerState.Normal; 
            }
            
        }
    }

    public void ShowAndHideVolumeSettings()
    {
        if(!volumeSettingsShown)
        {
            mouseSensPanel.SetActive(false);
            volumeSettingsPanel.SetActive(true);
            volumeSettingsShown = true;
        }
        else if(volumeSettingsShown)
        {
            volumeSettingsPanel.SetActive(false);
            mouseSensPanel.SetActive(true);
            volumeSettingsShown = false;
        }
    }

    public void OnSensitivityChanged(float value)
    {
        if (playerCameraMovement != null)
        {
            playerCameraMovement.SetSensitivity(value);
        }
    }
}
